package com.ahorrouy.repository.implementation

import android.util.Log
import com.ahorrouy.api.RetrofitInstance
import com.ahorrouy.api.model.marketMap.BestOptionResponse
import com.ahorrouy.repository.interfaces.IBestOptionRepository
import com.ahorrouy.ui.marketMap.model.ProductSearchModel
import okhttp3.MediaType.Companion.toMediaTypeOrNull
import okhttp3.ResponseBody.Companion.toResponseBody
import retrofit2.Response

class BestOptionRepository : IBestOptionRepository {
    override suspend fun bestOptionsToBuy(
        token: String,
        productsToBuy: Array<ProductSearchModel>,
        longitude: Float,
        latitude: Float,
        maxDistance: Int
    ): Response<List<BestOptionResponse>> {
        return try {
            RetrofitInstance.bestOptionEndpoint.bestOptionsToBuy(
                token,
                productsToBuy,
                longitude,
                latitude,
                maxDistance
            )
        } catch (ex: Exception) {
            Log.e("Exception on bestOptionsToBuy()", ex.message!!)
            Response.error(500, "SERVER_ERROR".toResponseBody("text/plain".toMediaTypeOrNull()))
        }
    }
}