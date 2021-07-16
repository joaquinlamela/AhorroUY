package com.ahorrouy.repository.implementation

import android.util.Log
import com.ahorrouy.api.RetrofitInstance
import com.ahorrouy.api.model.login.LoginRequest
import com.ahorrouy.api.model.login.LoginResponse
import com.ahorrouy.api.model.signup.SignupRequest
import com.ahorrouy.api.model.signup.SignupResponse
import okhttp3.MediaType.Companion.toMediaTypeOrNull
import okhttp3.ResponseBody.Companion.toResponseBody
import retrofit2.Response

class UserRepository {

    suspend fun postUser(signup: SignupRequest): Response<SignupResponse> {
        return try {
            RetrofitInstance.userEndpoints.postUser(signup)
        } catch (ex: Exception) {
            Log.e("Exception on Login", ex.message!!)
            Response.error(500, "SERVER_ERROR".toResponseBody("text/plain".toMediaTypeOrNull()))
        }
    }

    suspend fun login(credentials: LoginRequest): Response<LoginResponse>? {
        return try {
            RetrofitInstance.userEndpoints.login(credentials)
        } catch (ex: Exception) {
            Log.e("Exception on Login", ex.message!!)
            Response.error(500, "SERVER_ERROR".toResponseBody("text/plain".toMediaTypeOrNull()))
        }
    }

    suspend fun logout(token: String) {
        try {
            RetrofitInstance.userEndpoints.logout(token)
        } catch (ex: Exception) {
            Log.e("Exception on Login", ex.message!!)
        }
    }
}