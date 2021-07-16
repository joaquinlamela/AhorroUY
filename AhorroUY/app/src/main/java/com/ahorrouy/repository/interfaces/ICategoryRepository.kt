package com.ahorrouy.repository.interfaces

import com.ahorrouy.api.model.category.CategoryResponse
import retrofit2.Response

interface ICategoryRepository {
    suspend fun getCategoriesList(): Response<List<CategoryResponse>>
}