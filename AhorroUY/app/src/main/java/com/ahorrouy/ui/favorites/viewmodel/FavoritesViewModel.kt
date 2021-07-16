package com.ahorrouy.ui.favorites.viewmodel

import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.ahorrouy.api.model.product.ProductResponse
import com.ahorrouy.repository.implementation.FavoritesRepository
import kotlinx.coroutines.launch

class FavoritesViewModel(private val favoritesRepository: FavoritesRepository) : ViewModel() {

    val favorites: MutableLiveData<MutableList<ProductResponse>> by lazy {
        MutableLiveData<MutableList<ProductResponse>>().also {
            it.value = favoritesRepository.getFavorites()
        }
    }

    fun refresh(auth: String) {
        viewModelScope.launch {
            favorites.value = favoritesRepository.refresh(auth)
        }
    }

    fun add(auth: String, productId: String) {
        viewModelScope.launch {
            favoritesRepository.add(auth, productId)
            favorites.value = favoritesRepository.getFavorites()
        }
    }

    fun remove(auth: String, productId: String) {
        viewModelScope.launch {
            favoritesRepository.delete(auth, productId)
            favorites.value = favoritesRepository.getFavorites()
        }
    }

    fun clean() {
        favoritesRepository.clean()
        favorites.value = favoritesRepository.getFavorites()
    }

}