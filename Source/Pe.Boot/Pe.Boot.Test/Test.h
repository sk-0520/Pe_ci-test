﻿#pragma once
#include <vector>
#include <tuple>

#include <tchar.h>

#ifdef RES_CHECK
#   define text(s) mem_check__new_text(_T(s), _T(__FILE__), __LINE__)
#else
#   define text(s) new_text(_T(s))
#endif

#define wrap(s) wrap_text(_T(s))

namespace PeBootTest
{
    template<typename TExpected, typename TInput1, typename... TInputN>
    struct DATA
    {
#pragma region constructor

        DATA(TExpected expected, TInput1 input1, TInputN... inputN)
        {
            this->expected = expected;
            this->inputs = { input1, inputN... };
        }

#pragma endregion

#pragma region variable

        TExpected expected;
        std::tuple<TInput1, TInputN...> inputs;

#pragma endregion

    };

    template<typename TPrimitive>
    struct BOX
    {
        TPrimitive value;
    };
    using BOX_INT = BOX<int>;
    using BOX_SIZE_T = BOX<size_t>;

    template<typename TPrimitive>
    BOX<TPrimitive> create(TPrimitive value)
    {
        BOX<TPrimitive> result = {
            value,
        };
        return result;
    }
}


