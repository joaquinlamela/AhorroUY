package com.ahorrouy.repository.implementation

import android.util.Log
import com.ahorrouy.api.RetrofitInstance
import com.ahorrouy.api.model.category.CategoryResponse
import com.ahorrouy.repository.interfaces.ICategoryRepository
import okhttp3.MediaType.Companion.toMediaTypeOrNull
import okhttp3.ResponseBody.Companion.toResponseBody
import retrofit2.Response

class CategoryRepository : ICategoryRepository {
    override suspend fun getCategoriesList(): Response<List<CategoryResponse>> {
        return try {
            RetrofitInstance.categoryEndpoints.getAllCategories()
        } catch (ex: Exception) {
            Log.e("Exception on getCategoriesList()", ex.message!!)
            Response.error(500, "SERVER_ERROR".toResponseBody("text/plain".toMediaTypeOrNull()))
        }
    }

}