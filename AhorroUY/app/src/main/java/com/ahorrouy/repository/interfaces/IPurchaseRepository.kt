package com.ahorrouy.repository.interfaces

import com.ahorrouy.api.model.purchase.PurchaseModelRequest
import com.ahorrouy.api.model.purchase.PurchaseModelResponse
import retrofit2.Response

interface IPurchaseRepository {
    suspend fun postPurchase(
        auth: String,
        purchaseModelRequest: PurchaseModelRequest
    ): Response<PurchaseModelResponse>
}