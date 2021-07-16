package com.ahorrouy.api.endpoints

import com.ahorrouy.api.model.product.ProductDiscountResponse
import com.ahorrouy.api.model.product.ProductResponse
import retrofit2.Response
import retrofit2.http.GET
import retrofit2.http.Query


interface ProductWebService {
    @GET("products")
    suspend fun getProductsList(): Response<List<ProductResponse>>

    @GET("products/discountProducts")
    suspend fun getDiscountedProductsList(): Response<List<ProductDiscountResponse>>

    @GET("products/productsByText")
    suspend fun getProductsSearchedList(
        @Query("searchText") searchText: String,
        @Query("criteria") criteria: Int,
        @Query("categoryId") categoryId: String
    ): Response<List<ProductResponse>>

    @GET("products/productsByBarcode")
    suspend fun getProductByBarcode(
        @Query("barcode") barcode: String,
    ): Response<ProductResponse>
}