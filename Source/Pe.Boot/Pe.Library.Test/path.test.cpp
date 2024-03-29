﻿#include "pch.h"
#include <vector>

extern "C" {
#   include "../Pe.Library/path.h"
}

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace PeLibraryTest
{
    TEST_CLASS(path_test)
    {
    public:

        TEST_METHOD(is_directory_separator_test)
        {
            auto tests = {
                DATA(true, _T('\\')),
                DATA(true, _T('/')),
                DATA(false, _T('￥')),
                DATA(false, _T('¥')),
                DATA(false, _T('\0')),
            };
            for (auto test : tests) {
                bool actual = test.run(is_directory_separator);
                Assert::AreEqual(test.expected, actual);
            }
        }

        TEST_METHOD(has_root_path_test)
        {
            auto tests = {
                DATA(true, wrap("\\")),
                DATA(true, wrap("/")),
                DATA(true, wrap("C:\\")),
                DATA(true, wrap("C:")),
                DATA(true, wrap("C:/")),
                DATA(false, wrap("0:")),
                DATA(false, wrap("abc")),
            };
            for (auto test : tests) {
                TEXT arg1 = std::get<0>(test.inputs);

                bool actual = has_root_path(&arg1);
                Assert::AreEqual(test.expected, actual);
            }
        }

        TEST_METHOD(get_parent_directory_path_test)
        {
            auto tests = {
                DATA(wrap("C:\\dir\\file"), wrap("C:\\dir\\file\\")),
                DATA(wrap("C:\\dir"), wrap("C:\\dir\\file")),
                DATA(wrap("C:\\"), wrap("C:\\dir")),
                DATA(wrap(""), wrap("abc")),
                DATA(wrap("C:/"), wrap("C:/")),
                DATA(wrap("C:\\"), wrap("C:\\")),
                DATA(wrap(""), wrap("C:")),
                DATA(wrap(""), wrap("C")),
            };
            for (auto test : tests) {
                TEXT arg1 = std::get<0>(test.inputs);

                TEXT actual = get_parent_directory_path(&arg1, DEFAULT_MEMORY_ARENA);
                Assert::AreEqual(test.expected.value, actual.value);

                release_text(&actual);
            }
        }

        TEST_METHOD(split_path_test)
        {
            auto tests = {
                DATA(std::vector({ wrap("C:") }), wrap("C:")),
                DATA(std::vector({ wrap("C:"), wrap("dir") }), wrap("C:\\dir")),
                DATA(std::vector({ wrap("C:"), wrap("dir") }), wrap("C:\\\\dir")),
                DATA(std::vector({ wrap("C:"), wrap("dir") }), wrap("C:\\\\\\dir")),
                DATA(std::vector({ wrap("C:"), wrap("dir") }), wrap("C:/\\/\\/\\/dir")),
                DATA(std::vector({ wrap("C:") }), wrap("\\C:")),
                DATA(std::vector({ wrap("C:") }), wrap("\\C:\\")),
                DATA(std::vector({ wrap("C:") }), wrap("\\/\\/\\/\\/C:")),
                DATA(std::vector({ wrap("C:") }), wrap("C:\\/\\/\\/\\/")),
                DATA(std::vector({ wrap("a"), wrap("b"), wrap("c") }), wrap("////////////a/////////////b/////////////c////////////")),
                DATA(std::vector({ wrap("🥚"), wrap("🐤"), wrap("🐓"), wrap("🍗") }), wrap("🥚\\🐤\\🐓\\🍗")),
            };
            for (auto test : tests) {
                TEXT arg1 = std::get<0>(test.inputs);

                OBJECT_LIST actual = split_path(&arg1, DEFAULT_MEMORY_ARENA);

                Assert::AreEqual(test.expected.size(), actual.length);

                for (size_t i = 0; i < test.expected.size(); i++) {
                    OBJECT_RESULT_VALUE result = get_object_list(&actual, i);
                    Assert::IsTrue(result.exists);
                    const TEXT* actual_text = (TEXT*)result.value;

                    Assert::IsTrue(is_equals_text(&test.expected[i], actual_text, false), test.expected[i].value);
                }

                release_object_list(&actual, true);
            }
        }

        TEST_METHOD(canonicalize_path_test)
        {
            auto tests = {
                DATA(wrap("C:\\dir\\x\\file2"), wrap("C:\\dir\\file\\..\\x\\.\\file2")),
                DATA(wrap("a"), wrap("..\\..\\a")),
                DATA(wrap("a"), wrap("../../a")),
            };
            for (auto test : tests) {
                TEXT arg1 = std::get<0>(test.inputs);

                TEXT actual = canonicalize_path(&arg1, DEFAULT_MEMORY_ARENA);

                Assert::IsTrue(is_equals_text(&test.expected, &actual, true), test.expected.value);

                release_text(&actual);
            }
        }

        TEST_METHOD(combine_path_test)
        {
            auto tests = {
                DATA(wrap("C:\\dir\\file"), wrap("C:\\dir"), wrap("file")),
                DATA(wrap("a\\b"), wrap("a"), wrap("b")),
                DATA(wrap("a\\b"), wrap("a\\"), wrap("b\\")),
                DATA(wrap("a"), wrap("a"), wrap("")),
                DATA(wrap("b"), wrap(""), wrap("b")),
                DATA(wrap(""), wrap(""), wrap("")),
                DATA(wrap("\\a\\b"), wrap("\\a"), wrap("b")),
            };
            for (auto test : tests) {
                TEXT arg1 = std::get<0>(test.inputs);
                TEXT arg2 = std::get<1>(test.inputs);

                TEXT actual = combine_path(&arg1, &arg2, DEFAULT_MEMORY_ARENA);
                Assert::AreEqual(test.expected.value, actual.value);

                release_text(&actual);
            }
        }

        TEST_METHOD(join_path_test)
        {
            TEXT input1 = wrap("C:\\");
            TEXT input2[] = {
                wrap("dir1\\"),
                wrap("dir2"),
                wrap("."),
                wrap(".."),
                wrap("dir2+"),
                wrap(""),
            };
            TEXT expected = wrap("C:\\dir1\\dir2+");

            TEXT actual = join_path(&input1, input2, SIZEOF_ARRAY(input2), DEFAULT_MEMORY_ARENA);
            Assert::AreEqual(expected.value, actual.value);

            release_text(&actual);
        }

        TEST_METHOD(get_path_info_stack_test)
        {
            auto tests = {
                DATA(PATH_INFO{ wrap("C:\\dir"), wrap("file.ext"), wrap("file"), wrap("ext") }, wrap("C:\\dir\\file.ext")),
                DATA(PATH_INFO{ wrap("C:\\dir\\dir2"), wrap("file.ext"), wrap("file"), wrap("ext") }, wrap("C:\\dir\\dir2\\file.ext")),
                DATA(PATH_INFO{ wrap(""), wrap(""), wrap(""), wrap("") }, wrap("")),
                DATA(PATH_INFO{ wrap(""), wrap(""), wrap(""), wrap("") }, wrap("C:\\")),
                DATA(PATH_INFO{ wrap("C:\\dir"), wrap("abc"), wrap("abc"), wrap("") }, wrap("C:\\dir\\abc")),
                DATA(PATH_INFO{ wrap("C:\\dir"), wrap(".htaccess"), wrap(""), wrap("htaccess") }, wrap("C:\\dir\\.htaccess")),
                DATA(PATH_INFO{ wrap("C:"), wrap(".htaccess"), wrap(""), wrap("htaccess") }, wrap("C:\\.htaccess")),
                DATA(PATH_INFO{ wrap("C:"), wrap("dir"), wrap("dir"), wrap("") }, wrap("C:\\dir")),
                DATA(PATH_INFO{ wrap("C:\\dir"), wrap("a.txt.ext"), wrap("a.txt"), wrap("ext") }, wrap("C:\\dir\\a.txt.ext")),
            };
            for (auto test : tests) {
                TEXT& arg1 = std::get<0>(test.inputs);
                PATH_INFO actual = get_path_info_stack(&arg1);
                Assert::IsTrue(is_equals_text(&test.expected.parent, &actual.parent, false));
                Assert::IsTrue(is_equals_text(&test.expected.name, &actual.name, false));
                Assert::IsTrue(is_equals_text(&test.expected.name_without_extension, &actual.name_without_extension, false));
                Assert::IsTrue(is_equals_text(&test.expected.extension, &actual.extension, false));
            }
        }

        TEST_METHOD(clone_path_info_and_release_path_info_test)
        {
            PATH_INFO expected = PATH_INFO{ wrap("C:\\dir\\sub"), wrap("file.ext"), wrap("file"), wrap("ext") };
            PATH_INFO keep;
            {
                TEXT input = wrap("C:\\dir\\sub\\file.ext");
                PATH_INFO stack = get_path_info_stack(&input);
                keep = clone_path_info(&stack, DEFAULT_MEMORY_ARENA);
                Assert::IsFalse(release_path_info(&stack));
            }

            Assert::IsTrue(is_equals_text(&expected.parent, &keep.parent, false));
            Assert::IsTrue(is_equals_text(&expected.name, &keep.name, false));
            Assert::IsTrue(is_equals_text(&expected.name_without_extension, &keep.name_without_extension, false));
            Assert::IsTrue(is_equals_text(&expected.extension, &keep.extension, false));

            Assert::IsTrue(release_path_info(&keep));
        }
    };
}
