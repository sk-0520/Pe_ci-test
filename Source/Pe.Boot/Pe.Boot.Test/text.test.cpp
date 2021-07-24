﻿#include "pch.h"

extern "C" {
#   include "../Pe.Boot/text.h"
}

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace PeBootTest
{
    TEST_CLASS(text_test)
    {
    public:

        TEST_METHOD(new_test)
        {
            TCHAR input[] = _T("あいう");
            TEXT c = new_text(input);
            TEXT w = wrap_text(input);

            Assert::IsTrue(c.library.need_release);
            Assert::IsFalse(w.library.need_release);

            Assert::IsFalse(c.value == input);
            Assert::IsTrue(w.value == input);

            Assert::AreEqual(get_string_length(input), c.length);
            Assert::AreEqual(get_string_length(input), w.length);

            input[0] = _T('え');
            input[1] = _T('お');
            input[2] = 0;

            Assert::AreNotEqual(_T("えお"), c.value);
            Assert::AreEqual(_T("えお"), w.value);
            Assert::AreNotEqual(get_string_length(input), c.length);

            TEXT dc = clone_text(&c);
            TEXT dw = clone_text(&w);

            Assert::IsTrue(dc.library.need_release);
            Assert::IsTrue(dw.library.need_release);

            Assert::AreEqual(c.value, dc.value);
            Assert::AreEqual(w.value, dw.value);

            Assert::IsTrue(free_text(&c));
            Assert::IsFalse(free_text(&w));

            Assert::IsTrue(c.library.released);
            Assert::IsFalse(w.library.released);

            Assert::IsTrue(free_text(&dc));
            Assert::IsTrue(free_text(&dw));
        }

    };
}
