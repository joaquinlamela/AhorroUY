package com.ahorrouy.repository.implementation

import android.util.Log
import com.ahorrouy.api.RetrofitInstance
import com.ahorrouy.api.model.purchase.PurchaseModelRequest
import com.ahorrouy.api.model.purchase.PurchaseModelResponse
import com.ahorrouy.repository.interfaces.IPurchaseRepository
import okhttp3.MediaType.Companion.toMediaTypeOrNull
import okhttp3.ResponseBody.Companion.toResponseBody
import retrofit2.Response

class PurchaseRepository : IPurchaseRepository {

    override suspend fun postPurchase(
        auth: String,
        purchaseModelRequest: PurchaseModelRequest
    ): Response<PurchaseModelResponse> {
        return try {
            RetrofitInstance.purchaseEndpoint.postPurchase(auth, purchaseModelRequest)
        } catch (ex: Exception) {
            Log.e("Exception on postPurchase()", ex.message!!)
            Response.error(500, "SERVER_ERROR".toResponseBody("text/plain".toMediaTypeOrNull()))
        }
    }
}