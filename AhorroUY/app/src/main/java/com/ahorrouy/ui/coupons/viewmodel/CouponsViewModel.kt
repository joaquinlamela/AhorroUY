package com.ahorrouy.ui.coupons.viewmodel

import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.ahorrouy.api.model.coupon.CouponGetResponse
import com.ahorrouy.repository.implementation.CouponRepository
import kotlinx.coroutines.launch
import retrofit2.Response

class CouponsViewModel(private val couponRepository: CouponRepository) : ViewModel() {

    val coupons: MutableLiveData<Response<List<CouponGetResponse>>> = MutableLiveData()

    fun refresh(auth: String) {
        viewModelScope.launch {
            val response: Response<List<CouponGetResponse>> = couponRepository.getCoupons(auth)
            coupons.value = response
        }
    }
}