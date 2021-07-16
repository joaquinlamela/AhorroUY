package com.ahorrouy.repository.implementation

import android.util.Log
import com.ahorrouy.api.RetrofitInstance
import com.ahorrouy.api.model.token.TokenResponse
import com.ahorrouy.repository.interfaces.ITokenRepository
import okhttp3.MediaType.Companion.toMediaTypeOrNull
import okhttp3.ResponseBody.Companion.toResponseBody
import retrofit2.Response

class TokenRepository : ITokenRepository {
    override suspend fun postToken(token: String): Response<TokenResponse> {
        return try {
            RetrofitInstance.tokenEndpoint.postToken(token)
        } catch (ex: Exception) {
            Log.e("Exception on Post Token", ex.message!!)
            Response.error(500, "SERVER_ERROR".toResponseBody("text/plain".toMediaTypeOrNull()))
        }
    }
}