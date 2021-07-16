package com.ahorrouy.ui.search.model

import com.ahorrouy.api.Constants

data class Search(
    var productName: String = Constants.DEFAULT_PRODUCT_NAME,
    var criteria: Int = 0,
    var categoryId: String = Constants.DEFAULT_CATEGORY.id
)
