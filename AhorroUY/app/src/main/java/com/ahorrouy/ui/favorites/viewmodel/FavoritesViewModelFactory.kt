package com.ahorrouy.ui.favorites.viewmodel

import androidx.lifecycle.ViewModel
import androidx.lifecycle.ViewModelProvider
import com.ahorrouy.repository.implementation.FavoritesRepository

class FavoritesViewModelFactory(private val favoritesRepository: FavoritesRepository) :
    ViewModelProvider.Factory {

    override fun <T : ViewModel?> create(modelClass: Class<T>): T {
        return FavoritesViewModel(favoritesRepository) as T
    }

}