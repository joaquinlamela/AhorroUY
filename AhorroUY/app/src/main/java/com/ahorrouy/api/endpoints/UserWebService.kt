package com.ahorrouy.api.endpoints

import com.ahorrouy.api.model.login.LoginRequest
import com.ahorrouy.api.model.login.LoginResponse
import com.ahorrouy.api.model.signup.SignupRequest
import com.ahorrouy.api.model.signup.SignupResponse
import retrofit2.Response
import retrofit2.http.Body
import retrofit2.http.DELETE
import retrofit2.http.POST
import retrofit2.http.Path

interface UserWebService {
    @POST("Users")
    suspend fun postUser(
        @Body signup: SignupRequest
    ): Response<SignupResponse>

    @POST("Sessions")
    suspend fun login(
        @Body credentials: LoginRequest
    ): Response<LoginResponse>

    @DELETE("Sessions/{token}")
    suspend fun logout(@Path("token") token: String)

}