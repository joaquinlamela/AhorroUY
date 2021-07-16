package com.ahorrouy.repository.interfaces

import com.ahorrouy.api.model.product.ProductDiscountResponse
import com.ahorrouy.api.model.product.ProductResponse
import retrofit2.Response

interface IProductRepository {
    suspend fun getProductsList(): Response<List<ProductResponse>>
    suspend fun getDiscountedProductsList(): Response<List<ProductDiscountResponse>>
    suspend fun getProductsSearchedList(
        searchText: String,
        criteria: Int,
        categoryId: String
    ): Response<List<ProductResponse>>

    suspend fun getProductByBarcode(barcode: String): Response<ProductResponse>
}