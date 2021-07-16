package com.ahorrouy.api.endpoints

import com.ahorrouy.api.model.purchase.PurchaseModelRequest
import com.ahorrouy.api.model.purchase.PurchaseModelResponse
import retrofit2.Response
import retrofit2.http.Body
import retrofit2.http.Header
import retrofit2.http.POST

interface PurchaseWebService {
    @POST("purchase")
    suspend fun postPurchase(
        @Header("Auth") auth: String,
        @Body purchase: PurchaseModelRequest
    ): Response<PurchaseModelResponse>
}
