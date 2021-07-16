package com.ahorrouy.ui.home.viewmodel

import androidx.lifecycle.ViewModel
import androidx.lifecycle.ViewModelProvider
import com.ahorrouy.repository.implementation.FavoritesRepository
import com.ahorrouy.repository.interfaces.ICategoryRepository
import com.ahorrouy.repository.interfaces.IProductRepository
import com.ahorrouy.repository.interfaces.ITokenRepository

class HomeViewModelFactory(
    private val categoryRepo: ICategoryRepository,
    private val productRepo: IProductRepository,
    private val favoritesRepo: FavoritesRepository,
    private val tokenRepo: ITokenRepository
) : ViewModelProvider.Factory {

    override fun <T : ViewModel?> create(modelClass: Class<T>): T {
        return HomeViewModel(categoryRepo, productRepo, favoritesRepo, tokenRepo) as T
    }
}