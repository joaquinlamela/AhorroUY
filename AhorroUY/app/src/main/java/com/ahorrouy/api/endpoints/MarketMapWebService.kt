package com.ahorrouy.api.endpoints

import com.ahorrouy.api.model.marketMap.BestOptionResponse
import com.ahorrouy.ui.marketMap.model.ProductSearchModel
import retrofit2.Response
import retrofit2.http.Body
import retrofit2.http.Header
import retrofit2.http.POST
import retrofit2.http.Query

interface MarketMapWebService {
    @POST("bestOption")
    suspend fun bestOptionsToBuy(
        @Header("Auth") auth: String,
        @Body products: Array<ProductSearchModel>,
        @Query("longitude") longitude: Float,
        @Query("latitude") latitude: Float,
        @Query("maxDistance") maxDistance: Int
    ): Response<List<BestOptionResponse>>
}