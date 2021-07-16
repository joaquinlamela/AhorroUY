package com.ahorrouy.ui.search.viewmodel

import androidx.lifecycle.*
import com.ahorrouy.repository.interfaces.ICategoryRepository
import com.ahorrouy.repository.interfaces.IProductRepository
import com.ahorrouy.ui.search.model.Search
import kotlinx.coroutines.Dispatchers

class SearchProductViewModel(
    private val productRepo: IProductRepository,
    private val categoryRepo: ICategoryRepository
) : ViewModel() {

    private val productsData = MutableLiveData<Search>()
    private val barcodeData = MutableLiveData<String>()

    fun setParameters(searchParameters: Search) {
        productsData.value = searchParameters
    }

    fun setBarcode(barcode: String) {
        barcodeData.value = barcode
    }

    val fetchProductByBarcode = barcodeData.switchMap { barcodeData ->
        liveData(context = viewModelScope.coroutineContext + Dispatchers.IO) {
            emit(productRepo.getProductByBarcode(barcodeData))
        }
    }

    val fetchSearchedProductList = productsData.switchMap { searchParameters ->
        liveData(context = viewModelScope.coroutineContext + Dispatchers.IO) {
            emit(
                productRepo.getProductsSearchedList(
                    searchParameters.productName,
                    searchParameters.criteria,
                    searchParameters.categoryId
                )
            )
        }
    }

    val fetchCategoriesList =
        liveData(Dispatchers.IO) {
            emit(categoryRepo.getCategoriesList())
        }

}