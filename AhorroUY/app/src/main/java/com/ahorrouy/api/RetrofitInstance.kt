package com.ahorrouy.api

import com.ahorrouy.api.Constants.Companion.BASE_URL
import com.ahorrouy.api.endpoints.*
import retrofit2.Retrofit
import retrofit2.converter.gson.GsonConverterFactory

object RetrofitInstance {

    private val retrofit by lazy {
        Retrofit.Builder()
            .baseUrl(BASE_URL)
            .addConverterFactory(GsonConverterFactory.create())
            .build()
    }

    val userEndpoints: UserWebService by lazy {
        retrofit.create(UserWebService::class.java)
    }

    val categoryEndpoints: CategoryWebService by lazy {
        retrofit.create(CategoryWebService::class.java)
    }

    val productEndpoints: ProductWebService by lazy {
        retrofit.create(ProductWebService::class.java)
    }

    val tokenEndpoint: TokenWebService by lazy {
        retrofit.create(TokenWebService::class.java)
    }

    val couponEndpoints: CouponWebService by lazy {
        retrofit.create(CouponWebService::class.java)
    }

    val favoritesEndpoints: FavoritesWebService by lazy {
        retrofit.create(FavoritesWebService::class.java)
    }

    val bestOptionEndpoint: MarketMapWebService by lazy {
        retrofit.create(MarketMapWebService::class.java)
    }

    val purchaseEndpoint: PurchaseWebService by lazy {
        retrofit.create(PurchaseWebService::class.java)
    }
}