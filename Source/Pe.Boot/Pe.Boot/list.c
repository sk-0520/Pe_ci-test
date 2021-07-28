﻿#include "list.h"
#include "memory.h"
#include "debug.h"

/// <summary>
/// 指定型のバイト幅を取得。
/// </summary>
/// <param name="list_type"></param>
/// <returns></returns>
static size_t get_type_byte(PRIMITIVE_LIST_TYPE list_type)
{
    switch (list_type) {
        case PRIMITIVE_LIST_TYPE_UINT8:
            return 1;

        case PRIMITIVE_LIST_TYPE_UINT16:
            return 2;

        case PRIMITIVE_LIST_TYPE_UINT32:
            return 4;

        default:
            assert_debug(false);
    }

    return 0;
}

/// <summary>
/// 指定型の長さをバイト単位で取得。
/// </summary>
/// <param name="list_type"></param>
/// <param name="length"></param>
/// <returns></returns>
static size_t get_type_bytes(PRIMITIVE_LIST_TYPE list_type, size_t length)
{
    if (!length) {
        return 0;
    }
    return get_type_byte(list_type) * length;
}

PRIMITIVE_LIST RC_HEAP_FUNC(new_primitive_list, PRIMITIVE_LIST_TYPE list_type, size_t capacity)
{
    size_t capacity_bytes = get_type_bytes(list_type, capacity);
    void* buffer = RC_HEAP_CALL(allocate_memory, capacity_bytes, false);

    PRIMITIVE_LIST result = {
        .buffer = buffer,
        .length = 0,
        .library = {
            .capacity_bytes = capacity_bytes,
            .type = list_type,
    },
    };

    return result;
}

bool RC_HEAP_FUNC(free_primitive_list, PRIMITIVE_LIST* list)
{
    if (!list) {
        return false;
    }
    if (!list->buffer) {
        return false;
    }

    return RC_HEAP_CALL(free_memory, list->buffer);
}

// 同じような処理ばっか書いてんね
static size_t extend_capacity_if_not_enough_list(PRIMITIVE_LIST* list, size_t need_length)
{
    size_t need_bytes = get_type_bytes(list->library.type, need_length);
    size_t current_bytes = get_type_bytes(list->library.type, list->length);
    // まだ大丈夫なら何もしない
    size_t need_total_bytes = current_bytes + need_bytes;
    if (need_total_bytes <= list->library.capacity_bytes) {
        return 0;
    }

    size_t old_capacity_bytes = list->library.capacity_bytes;
    size_t new_capacity_bytes = list->library.capacity_bytes;
    do {
        new_capacity_bytes *= 2;
    } while (new_capacity_bytes < need_total_bytes);

    void* new_buffer = allocate_memory(new_capacity_bytes, false);
    void* old_buffer = list->buffer;

    copy_memory(new_buffer, old_buffer, new_capacity_bytes);
    free_memory(old_buffer);

    list->buffer = new_buffer;
    list->library.capacity_bytes = new_capacity_bytes;

    return new_capacity_bytes - old_capacity_bytes;
}

#define PRIMITIVE_ITEM(value_type, item) typedef struct tag_PRIMITIVE_ITEM_##item { value_type value; } PRIMITIVE_ITEM_##item
PRIMITIVE_ITEM(uint8_t, UINT8);
PRIMITIVE_ITEM(uint16_t, UINT16);
PRIMITIVE_ITEM(uint32_t, UINT32);

#define PUSH_PRIMITIVE_LIST_CORE(value_type, list_item) \
case PRIMITIVE_LIST_TYPE_##list_item: \
{\
    PRIMITIVE_ITEM_##list_item* x = (PRIMITIVE_ITEM_##list_item*)item; \
    extend_capacity_if_not_enough_list(list, get_type_byte(list->library.type)); \
    value_type* buffer = (value_type*)list->buffer; \
    buffer[list->length++] = x->value; \
    return true; \
}

static bool push_primitive_list_core(PRIMITIVE_LIST* list, void* item)
{
    switch (list->library.type) {
        /*
        case PRIMITIVE_LIST_TYPE_UINT8:
        {
            PRIMITIVE_ITEM_UINT8* uint8_item = (PRIMITIVE_ITEM_UINT8*)item;
            extend_capacity_if_not_enough_list(list, get_type_byte(list->library.type));
            uint8_t* buffer = (uint8_t*)list->buffer;
            buffer[list->length++] = uint8_item->value;
            return true;
        }
        */
        PUSH_PRIMITIVE_LIST_CORE(uint8_t, UINT8)
        PUSH_PRIMITIVE_LIST_CORE(uint16_t, UINT16)
        PUSH_PRIMITIVE_LIST_CORE(uint32_t, UINT32)

        default:
            assert_debug(false);
    }
    return false;
}

#define PUSH_PRIMITIVE_LIST_BODY(list_type, function, value_type, list_item) \
PUSH_PRIMITIVE_LIST_FUNC(list_type, function, value_type) \
{ \
    if(list->library.type != PRIMITIVE_LIST_TYPE_ ##list_item) { \
        return false; \
    } \
    PRIMITIVE_ITEM_##list_item  item = { value, }; \
    return push_primitive_list_core(list, &item); \
}

PUSH_PRIMITIVE_LIST_BODY(UINT8_LIST, uint8, uint8_t, UINT8);
PUSH_PRIMITIVE_LIST_BODY(UINT16_LIST, uint16, uint16_t, UINT16);
PUSH_PRIMITIVE_LIST_BODY(UINT16_LIST, uint32, uint32_t, UINT32);


#define GET_PRIMITIVE_LIST_BODY(list_type, function, value_type, list_item) \
GET_PRIMITIVE_LIST_FUNC(list_type, function, value_type) \
{ \
    if(list->library.type != PRIMITIVE_LIST_TYPE_ ##list_item) { \
        return false; \
    } \
    if(index < list->length) { \
        value_type* buffer = (value_type*)list->buffer; \
        *result = buffer[index]; \
    } \
    return false; \
}

GET_PRIMITIVE_LIST_BODY(UINT8_LIST, uint8, uint8_t, UINT8);
GET_PRIMITIVE_LIST_BODY(UINT16_LIST, uint16, uint16_t, UINT16);
GET_PRIMITIVE_LIST_BODY(UINT16_LIST, uint32, uint32_t, UINT32);

#define REFERENCE_PRIMITIVE_LIST_BODY(list_type, function, value_type, list_item) \
REFERENCE_PRIMITIVE_LIST_FUNC(list_type, function, value_type) \
{ \
    if(list->library.type != PRIMITIVE_LIST_TYPE_ ##list_item) { \
        return NULL; \
    } \
    return (value_type*)list->buffer; \
}
REFERENCE_PRIMITIVE_LIST_BODY(UINT8_LIST, uint8, uint8_t, UINT8);
REFERENCE_PRIMITIVE_LIST_BODY(UINT16_LIST, uint16, uint16_t, UINT16);
REFERENCE_PRIMITIVE_LIST_BODY(UINT32_LIST, uint32, uint32_t, UINT32);
