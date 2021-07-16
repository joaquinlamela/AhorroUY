package com.ahorrouy.api

import com.ahorrouy.api.model.category.CategoryResponse

class Constants {

    companion object {
        const val BASE_URL = "http:/192.168.68.119:6969/api/"
        val DEFAULT_CATEGORY =
            CategoryResponse("00000000-0000-0000-0000-000000000000", "", "", false)
        const val DEFAULT_PRODUCT_NAME = ""
        const val DEFAULT_BARCODE = ""
    }
}