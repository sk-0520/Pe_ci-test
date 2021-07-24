﻿#include <stddef.h>

#include "debug.h"
#include "memory.h"
#include "tstring.h"
#include "writer.h"

#define TRUE_UPPER "TRUE"
#define TRUE_LOWER "true"
#define FALSE_UPPER "FALSE"
#define FALSE_LOWER "false"

bool write_to_primitive_boolean(func_string_writer writer, void* receiver, bool value, bool is_uppper)
{
    WRITE_STRING_DATA data;
    if (value) {
        if (is_uppper) {
            data.value = _T(TRUE_UPPER);
            data.length = sizeof(TRUE_UPPER) - 1;
        } else {
            data.value = _T(TRUE_LOWER);
            data.length = sizeof(TRUE_LOWER) - 1;
        }
    } else {
        if (is_uppper) {
            data.value = _T(FALSE_UPPER);
            data.length = sizeof(FALSE_UPPER) - 1;
        } else {
            data.value = _T(FALSE_LOWER);
            data.length = sizeof(FALSE_LOWER) - 1;
        }
    }

    return writer(receiver, &data);
}

bool write_to_primitive_integer(func_string_writer writer, void* receiver, ssize_t value, WRITE_PADDING write_padding, WRITE_ALIGN write_align, bool show_sign, size_t width, TCHAR separator)
{
    TCHAR decimals[] = _T("0123456789");
    //size_t decimal_length = sizeof(decimals) / sizeof(TCHAR) - 1;

    TCHAR* buffer = allocate_clear_memory(sizeof(size_t) * 8 + 1 + width + ((sizeof(size_t) * 8) / 3), sizeof(TCHAR));
    size_t buffer_length = 0;
    bool is_negative = value < 0;
    ssize_t abs_value = is_negative ? -value : value;
    ssize_t number_of_digits = 0;
    do {
        int n = abs_value % 10;
        buffer[buffer_length++] = decimals[n];
        number_of_digits += 1;
        if (separator && (number_of_digits % 3) == 0) {
            buffer[buffer_length++] = separator;
        }
        abs_value /= 10;
    } while (abs_value != 0);

    size_t fill_buffer_index = buffer_length;
    if (show_sign || is_negative) {
        buffer_length += 1; // +-
    }
    size_t fill_buffer_length = MAX(buffer_length, width);


    if (buffer_length != fill_buffer_length) {
        if (write_align == WRITE_ALIGN_RIGHT || (write_align == WRITE_ALIGN_LEFT && write_padding == WRITE_PADDING_ZERO)) {
            TCHAR padding = _T('\0');
            switch (write_padding) {
                case WRITE_PADDING_ZERO:
                    padding = _T('0');
                    break;

                case WRITE_PADDING_SPACE:
                case WRITE_PADDING_NONE:
                    padding = _T(' ');
                    break;

                default:
                    assert_debug(false);
            }
            if (padding) {
                for (size_t i = fill_buffer_index; i < fill_buffer_length; i++) {
                    buffer[i] = padding;
                }
                if (show_sign || is_negative) {
                    if (write_padding == WRITE_PADDING_ZERO) {
                        buffer[fill_buffer_length - 1] = is_negative ? _T('-') : _T('+');
                    } else {
                        buffer[buffer_length - 1] = is_negative ? _T('-') : _T('+');
                    }
                }
            }
        } else {
            if (show_sign || is_negative) {
                buffer[buffer_length - 1] = is_negative ? _T('-') : _T('+');
            }
            fill_buffer_length = buffer_length;
        }
    } else {
        if (show_sign || is_negative) {
            buffer[buffer_length - 1] = is_negative ? _T('-') : _T('+');
        }
    }

    for (size_t left = 0, right = fill_buffer_length - 1; left < right; left++, right--) {
        TCHAR c = buffer[left];
        buffer[left] = buffer[right];
        buffer[right] = c;
    }

    WRITE_STRING_DATA data = {
        .value = buffer,
        .length = fill_buffer_length,
    };
    writer(receiver, &data);

    free_string(buffer);

    return true;
}
