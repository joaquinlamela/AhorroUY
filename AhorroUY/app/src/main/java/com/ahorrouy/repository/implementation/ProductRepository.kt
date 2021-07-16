package com.ahorrouy.repository.implementation

import android.util.Log
import com.ahorrouy.api.RetrofitInstance
import com.ahorrouy.api.model.product.ProductDiscountResponse
import com.ahorrouy.api.model.product.ProductResponse
import com.ahorrouy.repository.interfaces.IProductRepository
import okhttp3.MediaType.Companion.toMediaTypeOrNull
import okhttp3.ResponseBody.Companion.toResponseBody
import retrofit2.Response

class ProductRepository : IProductRepository {
    override suspend fun getProductsList(): Response<List<ProductResponse>> {
        return try {
            RetrofitInstance.productEndpoints.getProductsList()
        } catch (ex: Exception) {
            Log.e("Exception on getProductsList()", ex.message!!)
            Response.error(500, "SERVER_ERROR".toResponseBody("text/plain".toMediaTypeOrNull()))
        }
    }

    override suspend fun getDiscountedProductsList(): Response<List<ProductDiscountResponse>> {
        return try {
            RetrofitInstance.productEndpoints.getDiscountedProductsList()
        } catch (ex: Exception) {
            Log.e("Exception on getDiscountedProductsList()", ex.message!!)
            Response.error(500, "SERVER_ERROR".toResponseBody("text/plain".toMediaTypeOrNull()))
        }
    }

    override suspend fun getProductsSearchedList(
        searchText: String,
        criteria: Int,
        categoryId: String
    ): Response<List<ProductResponse>> {
        return try {
            RetrofitInstance.productEndpoints.getProductsSearchedList(
                searchText,
                criteria,
                categoryId
            )
        } catch (ex: Exception) {
            Log.e("Exception on getProductsSearchedList()", ex.message!!)
            Response.error(500, "SERVER_ERROR".toResponseBody("text/plain".toMediaTypeOrNull()))
        }
    }

    override suspend fun getProductByBarcode(barcode: String): Response<ProductResponse> {
        return try {
            RetrofitInstance.productEndpoints.getProductByBarcode(barcode)
        } catch (ex: Exception) {
            Log.e("Exception on getProductByBarcode()", ex.message!!)
            Response.error(500, "SERVER_ERROR".toResponseBody("text/plain".toMediaTypeOrNull()))
        }
    }
}