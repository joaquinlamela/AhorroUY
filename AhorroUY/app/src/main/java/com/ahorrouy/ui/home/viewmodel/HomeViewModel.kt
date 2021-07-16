package com.ahorrouy.ui.home.viewmodel

import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import androidx.lifecycle.liveData
import androidx.lifecycle.viewModelScope
import com.ahorrouy.api.model.token.TokenResponse
import com.ahorrouy.repository.implementation.FavoritesRepository
import com.ahorrouy.repository.interfaces.ICategoryRepository
import com.ahorrouy.repository.interfaces.IProductRepository
import com.ahorrouy.repository.interfaces.ITokenRepository
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import retrofit2.Response

class HomeViewModel(
    private val categoryRepo: ICategoryRepository,
    private val productRepo: IProductRepository,
    private val favoritesRepo: FavoritesRepository,
    private val tokenRepo: ITokenRepository
) : ViewModel() {

    private val productsData = MutableLiveData<String>()
    private var tokenResponse: MutableLiveData<Response<TokenResponse>> = MutableLiveData()

    fun setProduct(productName: String) {
        productsData.value = productName
    }

    val fetchCategoriesList =
        liveData(Dispatchers.IO) {
            emit(categoryRepo.getCategoriesList())
        }

    val fetchProductList =
        liveData(Dispatchers.IO) {
            val products = productRepo.getProductsList()
            val favorites = favoritesRepo.getFavorites()
            products.body()?.forEach { it.isFavorite = favorites.any { f -> f.id == it.id } }
            emit(products)
        }

    val fetchDiscountedProductList =
        liveData(Dispatchers.IO) {
            val products = productRepo.getDiscountedProductsList()
            val favorites = favoritesRepo.getFavorites()
            products.body()?.forEach { it.isFavorite = favorites.any { f -> f.id == it.id } }
            emit(products)
        }

    fun postToken(token: String) {
        viewModelScope.launch {
            val response: Response<TokenResponse> = tokenRepo.postToken(token)
            tokenResponse.value = response
        }
    }

}