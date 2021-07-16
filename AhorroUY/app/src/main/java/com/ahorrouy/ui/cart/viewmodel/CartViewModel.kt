package com.ahorrouy.ui.cart.viewmodel

import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import com.ahorrouy.repository.implementation.CartRepository
import com.ahorrouy.repository.model.CartProduct

class CartViewModel(private val cartRepository: CartRepository) : ViewModel() {

    val cart: MutableLiveData<MutableList<CartProduct>> by lazy {
        MutableLiveData<MutableList<CartProduct>>().also {
            it.value = cartRepository.getCart()
        }
    }

    fun add(product: CartProduct) {
        cartRepository.addProduct(product)
        cart.value = cartRepository.getCart()
    }

    fun remove(product: CartProduct) {
        cartRepository.removeProduct(product)
        cart.value = cartRepository.getCart()
    }

    fun changeQuantity(product: CartProduct) {
        cartRepository.newQuantity(product)
        cart.value = cartRepository.getCart()
    }

    fun clean() {
        cartRepository.clean()
        cart.value = cartRepository.getCart()
    }

}