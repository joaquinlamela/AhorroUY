package com.ahorrouy.ui.user.viewmodel

import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.ahorrouy.api.model.login.LoginRequest
import com.ahorrouy.api.model.login.LoginResponse
import com.ahorrouy.api.model.signup.SignupRequest
import com.ahorrouy.api.model.signup.SignupResponse
import com.ahorrouy.repository.implementation.UserRepository
import kotlinx.coroutines.launch
import retrofit2.Response

class UserViewModel(private val userRepository: UserRepository) : ViewModel() {

    data class CurrentUser(
        val Token: String,
        val Username: String,
        val Password: String
    )

    var signUpResponse: MutableLiveData<Response<SignupResponse>> = MutableLiveData()
    var loginResponse: MutableLiveData<Response<LoginResponse>> = MutableLiveData()

    var currentUser: MutableLiveData<CurrentUser> = MutableLiveData()

    fun postUser(signup: SignupRequest) {
        viewModelScope.launch {
            val response: Response<SignupResponse> = userRepository.postUser(signup)
            signUpResponse.value = response
        }
    }

    fun login(credentials: LoginRequest) {
        loginResponse.value = null
        viewModelScope.launch {
            val response = userRepository.login(credentials)
            loginResponse.value = response
        }
    }

    fun login(token: String, username: String) {
        if (token.length > 10)
            currentUser.value = CurrentUser(token, username, "")
    }

    fun logout() {
        if (currentUser.value != null)
            viewModelScope.launch {
                userRepository.logout(currentUser.value!!.Token)
            }
        currentUser.value = null
    }

    fun loginSuccess(token: String, credentials: LoginRequest) {
        currentUser.value = CurrentUser(token, credentials.Username, credentials.Password)
    }

}