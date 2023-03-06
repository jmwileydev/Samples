// test_bst.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include <iostream>
#include <binary_search_tree.h>
#include <vector>
#include <cassert>
#include <string>
#include <chrono>
#include <random>
#include <ranges>
#include <algorithm>
#include <format>
#include <set>


using namespace jms_utils;
using namespace std::string_literals;

//
// This generates a random seed from the milliseconds since epoch. Should be good enough for
// seeding my random generator
//
std::uint32_t get_seed()
{
    using namespace std::chrono;
    auto t = system_clock::now();
    auto ms = duration_cast<milliseconds>(t.time_since_epoch());
    return static_cast<std::uint32_t>(ms.count());
}

void test_bst()
{
    std::vector<int> small_tree_values{ 10, 4, 6, 13, 8, 22, 17, 14, 11, 27, 18 };

    // Try a small tree unbalanced
    {
        binary_search_tree<int> bst;

        for (auto i : small_tree_values)
        {
            auto [itr, inserted] = bst.insert(i);
            if (!inserted)
            {
                throw std::runtime_error("All values should have been unique");
            }
            else
            {
                assert(bst.find(i) != bst.end());
            }
#ifdef DEBUG
            bst.check_integrity();
#endif
        }

        // Once we have gotten there then try to iterate and make sure we get things in proper order
        std::cout << "The small test tree: ";
        std::optional<int> current_value;
        for (auto itr = bst.begin(); itr != bst.end(); itr++)
        {
            assert(!current_value || *current_value < *itr);
            if (itr != bst.begin())
            {
                std::cout << ", ";
            }

            std::cout << *itr;
        }

        std::cout << std::endl;

        std::cout << "The small test tree in reverse: ";
        current_value = std::nullopt;
        for (auto itr = bst.rbegin(); itr != bst.rend(); itr++)
        {
            assert(!current_value || *current_value > *itr);
            if (itr != bst.rbegin())
            {
                std::cout << ", ";
            }

            std::cout << *itr;
        }

        std::cout << std::endl;
    }

    // Try a small tree balanced
    {
        binary_search_tree<int, std::less<int>, true> bst;

        for (auto i : small_tree_values)
        {
            auto [itr, inserted] = bst.insert(i);
            if (!inserted)
            {
                throw std::runtime_error("All values should have been unique");
            }
#ifdef DEBUG
            bst.check_integrity();
#endif
        }

        // Once we have gotten there then try to iterate and make sure we get things in proper order
        std::cout << "The small test tree: ";
        for (auto itr = bst.begin(); itr != bst.end(); itr++)
        {
            if (itr != bst.begin())
            {
                std::cout << ", ";
            }

            std::cout << *itr;
        }

        std::cout << std::endl;
    }

    std::vector<std::string> str_vector{ "Hello", "World", "A", "Dog", "Is", "Awesome" };
    {
        binary_search_tree<std::string> bst;
        for (auto s : str_vector)
        {
            auto [itr, inserted] = bst.insert(s);
            if (!inserted)
            {
                throw std::runtime_error("All values should have been unique");
            }
#ifdef DEBUG
            bst.check_integrity();
#endif
        }

        // Once we have gotten there then try to iterate and make sure we get things in proper order
        std::cout << "The string tree: ";
        for (auto itr = bst.begin(); itr != bst.end(); itr++)
        {
            if (itr != bst.begin())
            {
                std::cout << ", ";
            }

            std::cout << *itr;
        }

        std::cout << std::endl;
    }


    std::cout << "Testing large trees now" << std::endl;
    // Now do some bigger tests using random numbers and verify everything is working ok.
    constexpr int test_size{ 100'000 };
    for (int i = 0; i < 10; i++)
    {
        // Let's first generate a list of 10,000 random numbers
        std::mt19937 gen; // Standard mersenne_twister_engine seeded with a random_device seed. Should be platform independent
        std::uniform_int_distribution<int> dist{ 0, test_size };
        std::vector<int> rvalues;
        rvalues.reserve(test_size);
        for (int j = 0; j < test_size; j++)
        {
            rvalues.push_back(dist(gen));
        }

        // generate an unbalanced tree first & check the integrity
        binary_search_tree<int> unbalanced_bst;
        for (auto k : rvalues)
        {
            auto [_itr, _inserted] = unbalanced_bst.insert(k);
        }
#ifdef DEBUG
        unbalanced_bst.check_integrity();
#endif
        // generate a balanced tree also & check the integrity
        binary_search_tree<int, std::less<int>, true> balanced_bst;
        for (auto k : rvalues)
        {
            auto [_itr, _inserted] = balanced_bst.insert(k);
        }
#ifdef DEBUG
        balanced_bst.check_integrity();
#endif

        // Let's use the std algorithms to sort and remove duplicates from our source vector
        std::ranges::sort(rvalues);
        auto [first, last] = std::ranges::unique(rvalues);
        rvalues.erase(first, last);

        // Now we should be able to traverse our trees in order
        auto unbalanced_itr = unbalanced_bst.begin();
        auto balanced_itr = balanced_bst.begin();
        for (auto v : rvalues)
        {
            assert(*unbalanced_itr++ == v);
            assert(*balanced_itr++ == v);
        }
        assert(unbalanced_itr == unbalanced_bst.end());
        assert(balanced_itr == balanced_bst.end());

        std::cout << "Tree number " << i << " complete" << std::endl;
    }
}

enum class Command
{
    TestBst,
    Performance,
    Help,
};

Command GetCommand(int argc, char* argv[])
{
    // This routine will simply see which command we have chosen and return it.
    Command command{ Command::TestBst };
    if (argc > 2)
    {
        std::cerr << "Error: Too many command line arguments, --help for help" << std::endl;
    }

    if (argc == 2 && _stricmp("--testbst", argv[1]) != 0)
    {
        if (_stricmp("--performance", argv[1]) == 0)
        {
            command = Command::Performance;
        }
        else if (_stricmp("--help", argv[1]) == 0)
        {
            command = Command::Help;
        }
        else
        {
            std::cout << "Unrecognized command line option " << argv[1] << std::endl;
            throw std::runtime_error("Bad command line");
        }
    }

    return command;
}

void run_performance_test()
{
    constexpr int test_size{ 1'000'000 };
    // Let's first generate a list of random integers
    std::mt19937 gen; // Standard mersenne_twister_engine seeded with a random_device seed. Should be platform independent
    std::uniform_int_distribution<int> dist{ 0, test_size };

    std::vector<int> values;
    values.resize(test_size);
    for (int i = 0; i < test_size; i++)
    {
        values.push_back(dist(gen));
    }

    // See how long it takes to put these entries into my tree
    {
        binary_search_tree<int, std::less<int>, true> bst;
        auto start = std::chrono::steady_clock::now();
        for (auto i : values)
        {
            bst.insert(i);
        }
        auto end = std::chrono::steady_clock::now();
        auto ms = std::chrono::duration_cast<std::chrono::milliseconds>(end - start).count();
        auto seconds = ms / 1000;
        ms = ms % 1000;
        auto min = seconds / 60;
        seconds = seconds % 60;

        // format the time as minutes::seconds::milliseconds
        std::cout << "Time taken BST ";
        std::cout << std::format("{:2}:{:2}:{:3}", min, seconds, ms) << std::endl;
    }

    {
        std::set<int> s;
        auto start = std::chrono::steady_clock::now();
        for (auto i : values)
        {
            s.insert(i);
        }
        auto end = std::chrono::steady_clock::now();
        auto ms = std::chrono::duration_cast<std::chrono::milliseconds>(end - start).count();
        auto seconds = ms / 1000;
        ms = ms % 1000;
        auto min = seconds / 60;
        seconds = seconds % 60;

        // format the time as minutes::seconds::milliseconds
        std::cout << "Time taken std::set: ";
        std::cout << std::format("{:2}:{:2}:{:3}", min, seconds, ms) << std::endl;
    }

}

int main(int argc, char* argv[])
{
    auto command = GetCommand(argc, argv);
    switch (command)
    {
    case Command::Help:
        std::cout << "Usage: test_bst --help|--testbst[default]|--performance]" << std::endl;
        std::cout << "\t--help - this message" << std::endl;
        std::cout << "\t--testbst - runs a bunch of tests to check the code. Should use a Debug build" << std::endl;
        std::cout << "\t--performance - runs a comparision between my bst and the std::set" << std::endl;
        break;
    case Command::TestBst:
        test_bst();
        break;
    case Command::Performance:
        run_performance_test();
        break;
    }

}