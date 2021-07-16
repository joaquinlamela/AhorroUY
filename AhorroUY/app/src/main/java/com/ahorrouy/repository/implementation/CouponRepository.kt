package com.ahorrouy.repository.implementation

import android.util.Log
import com.ahorrouy.api.RetrofitInstance
import com.ahorrouy.api.model.coupon.CouponGetResponse
import okhttp3.MediaType.Companion.toMediaTypeOrNull
import okhttp3.ResponseBody.Companion.toResponseBody
import retrofit2.Response

class CouponRepository {
    suspend fun getCoupons(auth: String): Response<List<CouponGetResponse>> {
        return try {
            RetrofitInstance.couponEndpoints.getCoupons(auth)
        } catch (ex: Exception) {
            Log.e("Exception on Login", ex.message!!)
            Response.error(500, "SERVER_ERROR".toResponseBody("text/plain".toMediaTypeOrNull()))
        }
    }
}