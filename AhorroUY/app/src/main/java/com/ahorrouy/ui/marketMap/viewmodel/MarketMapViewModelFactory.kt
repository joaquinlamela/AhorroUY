package com.ahorrouy.ui.marketMap.viewmodel

import androidx.lifecycle.ViewModel
import androidx.lifecycle.ViewModelProvider
import com.ahorrouy.repository.interfaces.IBestOptionRepository
import com.ahorrouy.repository.interfaces.IPurchaseRepository


class MarketMapViewModelFactory(
    private val marketMapRepository: IBestOptionRepository,
    private val purchaseRepository: IPurchaseRepository
) : ViewModelProvider.Factory {

    override fun <T : ViewModel?> create(modelClass: Class<T>): T {
        return modelClass.getConstructor(
            IBestOptionRepository::class.java,
            IPurchaseRepository::class.java
        ).newInstance(marketMapRepository, purchaseRepository)
    }
}