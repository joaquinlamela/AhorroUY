package com.ahorrouy.ui.search.viewmodel

import androidx.lifecycle.ViewModel
import androidx.lifecycle.ViewModelProvider
import com.ahorrouy.repository.interfaces.ICategoryRepository
import com.ahorrouy.repository.interfaces.IProductRepository

class SearchProductViewModelFactory(
    private val productRepo: IProductRepository,
    private val categoryRepo: ICategoryRepository
) : ViewModelProvider.Factory {

    override fun <T : ViewModel?> create(modelClass: Class<T>): T {
        return SearchProductViewModel(productRepo, categoryRepo) as T
    }
}