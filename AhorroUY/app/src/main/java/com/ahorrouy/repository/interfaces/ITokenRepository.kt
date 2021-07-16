package com.ahorrouy.repository.interfaces

import com.ahorrouy.api.model.token.TokenResponse
import retrofit2.Response

interface ITokenRepository {
    suspend fun postToken(token: String): Response<TokenResponse>
}