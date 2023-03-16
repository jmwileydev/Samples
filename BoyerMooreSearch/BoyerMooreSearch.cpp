// BoyerMooreSearch.cpp : This file contains the 'main' function. Program execution begins and ends there.
//
#include <iostream>
#include <optional>
#include <cassert>
#include <vector>
#include <thread>
#include <chrono>
#include <fstream>

char* BoyerMooreSearch(char* str, char* pattern)
{
    size_t pattern_length = strlen(pattern) - 1;

    // Skip out in str until we reach the end of the pattern. It saves us from having to ever call
    // strlen on the input str.
    auto current_end = str;
    for (size_t current_index = 0; current_index < pattern_length && *current_end != '\0'; current_index++)
    {
        current_end++;
    }

    auto pattern_end = pattern + pattern_length;
    while (*current_end != '\0')
    {
        auto p = pattern_end;
        auto s = current_end;

        // try and match all but the first character in the pattern
        while (*p == *s && p != pattern)
        {
            p--; s--;
        }

        if (*p == *s)
        {
            // Then we have found our pattern
            return s;
        }

        // We need to move our pattern so let's see if we can find a match
        p = pattern_end - 1;
        while (p != pattern && *p != *s)
        {
            p--;
        }

        if (*p != *s)
        {
            // we did not find the character we need to skip the entire thing
            for (size_t current_index = 0; current_index < pattern_length && *current_end != '\0'; current_index++)
            {
                current_end++;
            }
            current_end++;
            
            // NOTE: we could have *current_end == '\0 which means we did not find it. That is handled by the case at the top
            // of the while loop.
        }
        else
        {
            // We found the character. Now we need to slid current end
            while (p != pattern_end && *current_end != '\0')
            {
                current_end++;
                p++;
            }
        }
    }

    return nullptr;


}

void Usage()
{
    std::cout << "BoyerMooreSearch -Pattern <pattern> -Filename <file>" << std::endl;
    std::cout << "-Pattern - the pattern to search for" << std::endl;
    std::cout << "-Filename - file name to search for the pattern in, defaults to std::in" << std::endl;
}

int main(int argc, char* argv[])
{
    char* pattern{ nullptr };
    char* filename{ nullptr };

    try
    {
        for (int i = 1; i < argc; i++)
        {
            if (_stricmp("-Pattern", argv[i]) == 0)
            {
                ++i;
                if (i == argc)
                {
                    std::cerr << "A pattern needs to be supplied when the -Pattern argument is supplied" << std::endl;
                    Usage();
                    throw std::runtime_error("incorrect command line");
                }
                pattern = argv[i];
            }
            else if (_stricmp("-Filename", argv[i]) == 0)
            {
                ++i;
                if (i == argc)
                {
                    std::cerr << "A file name needs to be supplied when the -Filename argument is supplied" << std::endl;
                    Usage();
                    throw std::runtime_error("incorrect command line");
                }
                filename = argv[i];
            }
            else
            {
                std::cerr << "Unexpected command line" << std::endl;
                Usage();
                throw std::runtime_error("incorrect command line");
            }
        }
    }
    catch (std::runtime_error&)
    {
        return 1;
    }

    char* data{ nullptr };
    if (!filename || !pattern)
    {
        std::cerr << "A pattern and a file name must be supplied" << std::endl;
        Usage();
        return 1;
    }

    std::unique_ptr<char> sp;
    try
    {
        std::ifstream file_in(filename);
        file_in.seekg(0, std::ios_base::end);
        size_t size = file_in.tellg();
        file_in.seekg(0, std::ios_base::beg);
        std::unique_ptr<char> p(new char[size + 1]());
        char* data = p.get();
        constexpr std::streamsize bytes_per_read = 4 * 1024;
        do
        {
            file_in.read(data, bytes_per_read);
            data += file_in.gcount();
        } while (!file_in.eof());

        // Make sure we add a null terminator
        *data = 0;
        sp.swap(p);
    }
    catch (std::exception& e)
    {
        std::cerr << "Error: " <<  e.what() << " while reading file " 
            << filename << std::endl;
        return 1;
    }
   
    auto p = BoyerMooreSearch(sp.get(), pattern);

    // Print out how far into the file the pattern was found
    if (!p)
    {
        std::cout << "Pattern was not found in the file" << std::endl;
    }
    else
    {
        std::cout << "Pattern found starting at byte: " << (p - sp.get()) / sizeof(char) << std::endl;
    }
}

