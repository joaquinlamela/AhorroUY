package com.ahorrouy.ui.marketMap.viewmodel

import androidx.lifecycle.*
import com.ahorrouy.api.model.purchase.PurchaseModelRequest
import com.ahorrouy.api.model.purchase.PurchaseModelResponse
import com.ahorrouy.repository.interfaces.IBestOptionRepository
import com.ahorrouy.repository.interfaces.IPurchaseRepository
import com.ahorrouy.ui.marketMap.model.BestOptionRequestModel
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import retrofit2.Response

class MarketMapViewModel(
    private val bestOptionRepo: IBestOptionRepository,
    private val purchaseRepository: IPurchaseRepository
) : ViewModel() {

    private val dataToGetOptions = MutableLiveData<BestOptionRequestModel>()
    var purchaseResponse: MutableLiveData<Response<PurchaseModelResponse>> = MutableLiveData()


    fun setData(dataToGetMarkets: BestOptionRequestModel) {
        dataToGetOptions.value = dataToGetMarkets
    }

    val fetchOptionsToBuy = dataToGetOptions.switchMap { getMarketsParameters ->
        liveData(context = viewModelScope.coroutineContext + Dispatchers.IO) {
            emit(
                bestOptionRepo.bestOptionsToBuy(
                    getMarketsParameters.token,
                    getMarketsParameters.productsToBuy,
                    getMarketsParameters.longitude,
                    getMarketsParameters.latitude,
                    getMarketsParameters.maxDistance
                )
            )
        }
    }

    fun postPurchase(auth: String, purchase: PurchaseModelRequest) {
        viewModelScope.launch {
            val response: Response<PurchaseModelResponse> =
                purchaseRepository.postPurchase(auth, purchase)
            purchaseResponse.value = response
        }
    }
}