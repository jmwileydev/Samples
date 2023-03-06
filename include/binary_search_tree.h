#pragma once
#include <iterator>
#include <cstddef>
#include <stdexcept>
#include <list>
#include <memory>
#include <queue>
#include <set>
#include <cassert>
#include <optional>
#include <vector>
#include <shared_mutex>
#include <functional>

//
// binary_search_tree - this is a templated class for a binary search tree. For simplicity I am going to assume
// for now that the type has a operator < and an operator > that can be used for doing the comparison.
//
namespace jms_utils
{
    template<class T, class Comparer = std::less<T>, bool is_balanced = false>
    class binary_search_tree
    {
    private:
        struct binary_search_tree_node
        {
            T value;
            binary_search_tree_node* left{ nullptr };
            binary_search_tree_node* right{ nullptr };
            binary_search_tree_node* parent{ nullptr };
            int height{ 1 };

            ~binary_search_tree_node()
            {
            }
        };

        struct memory_node
        {
        private:
            binary_search_tree_node* _ptr;
            size_t _max_elements;
            size_t _next{ 0 };

        public:
            memory_node(size_t alloc_size = 4096)
            {
                _ptr = static_cast<binary_search_tree_node*>(malloc(alloc_size));
                _max_elements = alloc_size / sizeof(binary_search_tree_node);
            }

            ~memory_node()
            {
                auto p = _ptr;
                for (size_t i = 0; i < _next; i++)
                {
                    (p + i)->~binary_search_tree_node();
                }
            }

            binary_search_tree_node* get_next_node()
            {
                if (_next == _max_elements)
                {
                    return nullptr;
                }

                auto p = new(_ptr + _next) binary_search_tree_node();
                _next++;

                return p;
            }
        };

        binary_search_tree_node* _root;
        Comparer _cmp;
        static const size_t small_alloc_size{ 1 << 10 };
        static const size_t large_alloc_size{ 1 << 16 };
        size_t _alloc_size{ small_alloc_size };
        std::vector<std::unique_ptr<memory_node>> _memory_nodes;
        std::shared_mutex _mutex;

        binary_search_tree_node* get_next_node()
        {
            binary_search_tree_node* p;

            // check and see if we need to allocate a new node
            if (_memory_nodes.empty() || (p = _memory_nodes.back()->get_next_node()) == nullptr)
            {
                _memory_nodes.emplace_back(std::make_unique<memory_node>(_alloc_size));
                p = _memory_nodes.back()->get_next_node();
            }

            return p;
        }

        static binary_search_tree_node* find_first(binary_search_tree_node* node)
        {
            // This routine will simply move as far left as it can
            while (node->left)
            {
                node = node->left;
            }

            // Now I should be at a node w/o a left child
            return node;
        }

        static binary_search_tree_node* find_last(binary_search_tree_node* node)
        {
            // This routine will simply move as far left as it can
            while (node->right)
            {
                node = node->right;
            }

            // Now I should be at a node w/o a left child
            return node;
        }

        int height_of_node(binary_search_tree_node* p)
        {
            return p ? p->height : 0;
        }

        int height_of_children(binary_search_tree_node* p)
        {
            auto lh = height_of_node(p->left);
            auto rh = height_of_node(p->right);
            return std::max(lh, rh);
        }

        //
        // rotate_right
        //          p                  p
        //         /                  /
        //        n                  y
        //      /   \              /   \
        //     y      x     ->    a      n
        //    / \                       / \
        //   a   b                     b   x

        void rotate_right(binary_search_tree_node* node)
        {
            auto y = node->left;
            auto b = y->right;
            auto parent = node->parent;
            node->parent = y;
            y->parent = parent;
            node->left = b;
            if (b)
            {
                b->parent = node;
            }
            y->right = node;
            node->height = height_of_children(node) + 1;
            if (parent)
            {
                if (_cmp(y->value, parent->value))
                {
                    parent->left = y;
                }
                else
                {
                    parent->right = y;
                }
            }
            else
            {
                _root = y;
            }
        }

        //
        // rotate_left
        //        n                  y
        //      /   \              /   \
        //     x      y     ->    n      b
        //           / \         / \
        //          a   b       x   a
        void rotate_left(binary_search_tree_node* node)
        {
            auto y = node->right;
            auto a = y->left;
            auto parent = node->parent;
            node->parent = y;
            y->parent = parent;
            node->right = a;
            if (a)
            {
                a->parent = node;
            }
            y->left = node;
            node->height = height_of_children(node) + 1;
            if (parent)
            {
                if (_cmp(y->value, parent->value))
                {
                    parent->left = y;
                }
                else
                {
                    parent->right = y;
                }
            }
            else
            {
                _root = y;
            }
        }

    public:
        class iterator
        {
        private:
            binary_search_tree_node* _node{ nullptr };
            bool _visited_successor{ false };
            bool _is_forward{ true };
 
            bool has_successor()
            {
                return (_is_forward && _node->right) || (!_is_forward && _node->left);
            }

            bool is_parent_successor()
            {
                return (_is_forward && _node == _node->parent->left) ||
                    (!_is_forward && _node == _node->parent->right);
            }

        public:
            typedef std::input_iterator_tag iterator_category;
            typedef T value_type;
            typedef T* pointer_type;
            typedef T& reference;
            typedef iterator self_type;

            iterator(binary_search_tree_node* node, bool is_forward = true) :
                _node(node), _is_forward(is_forward) {};
            iterator(iterator const& itr) = default;
            iterator& operator=(iterator const& itr) = default;
            iterator() = default;
            ~iterator() = default;

            self_type& operator++() // prefix operator
            {
                if (_node == nullptr)
                {
                    throw std::runtime_error("Past end()");
                }

                 bool found{ false };
                while (!found)
                {
                    if (!_visited_successor && has_successor()) // if I have not visited my successor yet then go there.
                    {
                        _node = _is_forward ? find_first(_node->right) : find_last(_node->left);
                        _visited_successor = false;
                        found = true;
                    }
                    else if (!_node->parent)                // I don't have a parent so I am at the end
                    {
                        _node = nullptr;
                        _visited_successor = false;
                        found = true;
                    }
                    else if (is_parent_successor()) // I my parent succeeds me
                    {
                        _node = _node->parent;
                        _visited_successor = false;
                        found = true;
                    }
                    else
                    {
                        _visited_successor = true;
                        _node = _node->parent;
                    }
                }

                return *this;
            }


            self_type operator++(int)   // postfix operator
            {
                self_type tmp = *this;  // make a copy of my self.
                ++* this;               // then increment
                return tmp;
            }

            bool operator==(self_type const& other) const
            {
                return _node == other._node;
            }

            bool operator!=(self_type const& other) const
            {
                return _node != other._node;
            }

            reference operator*() const
            {
                if (!_node)
                {
                    throw std::invalid_argument("cannot dereference end()");
                }

                return _node->value;
            }

            reference operator->() const
            {
                if (!_node)
                {
                    throw std::invalid_argument("cannot dereference end()");
                }

                return _node->value;
            }
        };

        iterator begin()
        {
            std::shared_lock lock(_mutex);
            if (!_root)
            {
                return iterator();
            }

            return iterator(find_first(_root));
        }

        iterator rbegin()
        {
            std::shared_lock lock(_mutex);
            if (!_root)
            {
                return iterator();
            }

            return iterator(find_last(_root));
        }

        iterator end()
        {
            return iterator();
        }

        iterator rend()
        {
            return iterator();
        }

        iterator find(T const& value)
        {
            std::shared_lock lock(_mutex);
            auto p = _root;
            while (p)
            {
                if (_cmp(value, p->value))
                {
                    p = p->left;
                }
                else if (_cmp(p->value, value))
                {
                    p = p->right;
                }
                else
                {
                    return iterator(p);
                }
            }

            return iterator();
        }

        binary_search_tree()
        {
            // We try to do 4K blocks, if we can't hold at least 100 items then we switch to 64K
            auto alloc_units = _alloc_size / sizeof(T);
            if (alloc_units < 100)
            {
                _alloc_size = large_alloc_size;
            }
        }

        // Insert always works the same. It adds the node to the appropriate location in the tree. If the
        // tree is balanced it will then perform the needed steps to rebalance the tree if needed
        std::pair<iterator, bool> insert(T const& value)
        {
            std::unique_lock lock(_mutex);
            if (!_root)
            {
                _root = get_next_node();
                _root->value = value;
                return { iterator(_root), true };
            }

            std::optional<std::pair<binary_search_tree_node*, bool>> res;
            binary_search_tree_node* p = _root;
            while (!res)
            {
                if (_cmp(value, p->value)) // move left if we can
                {
                    if (!p->left)
                    {
                        p->left = get_next_node();
                        p->left->value = value;
                        p->left->parent = p;
                        res = { p->left, true };
                    }
                    else
                    {
                        p = p->left;
                    }
                }
                else if (_cmp(p->value, value))
                {
                    if (!p->right)
                    {
                        p->right = get_next_node();;
                        p->right->value = value;
                        p->right->parent = p;
                        res = { p->right, true };
                    }
                    else
                    {
                        p = p->right;
                    }
                }
                else
                {
                    res = { p, false };
                }
            }

            // If we inserted the node then we need to fix all of the parents
            
            auto& [node, inserted] {*res};
            if (inserted)
            {
                while (node->parent)
                {
                    node = node->parent;
                    node->height = height_of_children(node) + 1;

                    if (is_balanced)
                    {
                        // We need to check the heights and make sure we are still balanced
                        if (std::abs(height_of_node(node->left) - height_of_node(node->right)) >= 2)
                        {
                            // Are we left
                            if (_cmp(value, node->value))
                            {
                                if (_cmp(value, node->left->value))   // this is the left left case. Simply rotate right at this node
                                {
                                    rotate_right(node);
                                }
                                else
                                {
                                    rotate_left(node->left);
                                    rotate_right(node);
                                }
                            }
                            else
                            {
                                if (!_cmp(value, node->right->value))  // this is the right right case.
                                {
                                    rotate_left(node);
                                }
                                else
                                {
                                    rotate_right(node->right);
                                    rotate_left(node);
                                }
                            }
                        }
                    }
                }
            }

            return { iterator(node), inserted };
        }

#ifdef _DEBUG
        void check_integrity()
        {
            // This code will check each and every node to make sure that the children are
            // correct and that the heights are correct.
            std::queue<binary_search_tree_node*> q;
            q.push(_root);
            while (!q.empty())
            {
                auto p = q.front();
                q.pop();

                if (p)
                {
                    auto lh = height_of_node(p->left);
                    auto rh = height_of_node(p->right);
                    assert(p->height = std::max(lh, rh) + 1);
                    assert(!p->left || _cmp(p->left->value, p->value));
                    assert(!p->right || !_cmp(p->right->value, p->value));
                    assert(!is_balanced || std::abs(lh - rh) < 2);
                    q.push(p->left);
                    q.push(p->right);
                }
            }
        }
#endif
    };

}