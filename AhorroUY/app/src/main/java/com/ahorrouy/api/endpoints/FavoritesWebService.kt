package com.ahorrouy.api.endpoints

import com.ahorrouy.api.model.product.ProductResponse
import retrofit2.Response
import retrofit2.http.*

interface FavoritesWebService {

    @GET("Favorites")
    suspend fun getFavorites(
        @Header("Auth") auth: String
    ): Response<List<ProductResponse>>

    @POST("Favorites")
    suspend fun postFavorites(
        @Header("Auth") auth: String,
        @Query("productId") productId: String,
    ): Response<ProductResponse>

    @DELETE("Favorites")
    suspend fun deleteFavorites(
        @Header("Auth") auth: String,
        @Query("productId") productId: String,
    )
}