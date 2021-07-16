package com.ahorrouy.api.endpoints

import com.ahorrouy.api.model.category.CategoryResponse
import retrofit2.Response

import retrofit2.http.GET

interface CategoryWebService {

    @GET("categories")
    suspend fun getAllCategories(): Response<List<CategoryResponse>>

}