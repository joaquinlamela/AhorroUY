package com.ahorrouy.api.endpoints

import com.ahorrouy.api.model.coupon.CouponGetResponse
import retrofit2.Response
import retrofit2.http.GET
import retrofit2.http.Header

interface CouponWebService {
    @GET("Coupons")
    suspend fun getCoupons(
        @Header("Auth") auth: String
    ): Response<List<CouponGetResponse>>
}