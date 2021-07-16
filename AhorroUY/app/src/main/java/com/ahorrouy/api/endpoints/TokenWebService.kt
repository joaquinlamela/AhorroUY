package com.ahorrouy.api.endpoints

import com.ahorrouy.api.model.token.TokenResponse
import retrofit2.Response
import retrofit2.http.Body
import retrofit2.http.POST

interface TokenWebService {
    @POST("tokens")
    suspend fun postToken(@Body token: String): Response<TokenResponse>
}