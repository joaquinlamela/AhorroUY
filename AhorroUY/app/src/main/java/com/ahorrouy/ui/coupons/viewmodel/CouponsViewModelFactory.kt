package com.ahorrouy.ui.coupons.viewmodel

import androidx.lifecycle.ViewModel
import androidx.lifecycle.ViewModelProvider
import com.ahorrouy.repository.implementation.CouponRepository

class CouponsViewModelFactory(private val couponRepository: CouponRepository) :
    ViewModelProvider.Factory {

    override fun <T : ViewModel?> create(modelClass: Class<T>): T {
        return CouponsViewModel(couponRepository) as T
    }

}