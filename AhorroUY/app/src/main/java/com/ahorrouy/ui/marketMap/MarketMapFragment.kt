package com.ahorrouy.ui.marketMap

import android.app.Activity
import android.content.ContentValues.TAG
import android.content.Context
import android.graphics.Bitmap
import android.graphics.Canvas
import android.location.Location
import android.os.Bundle
import android.util.Log
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.AdapterView
import android.widget.ListView
import androidx.appcompat.widget.Toolbar
import androidx.core.content.ContextCompat
import androidx.fragment.app.Fragment
import androidx.fragment.app.activityViewModels
import androidx.fragment.app.viewModels
import androidx.lifecycle.Observer
import androidx.navigation.fragment.findNavController
import com.ahorrouy.R
import com.ahorrouy.api.model.marketMap.BestOptionResponse
import com.ahorrouy.api.model.purchase.PurchaseModelRequest
import com.ahorrouy.databinding.FragmentMapBinding
import com.ahorrouy.repository.implementation.BestOptionRepository
import com.ahorrouy.repository.implementation.PurchaseRepository
import com.ahorrouy.ui.cart.viewmodel.CartViewModel
import com.ahorrouy.ui.marketMap.adapter.MyAdapter
import com.ahorrouy.ui.marketMap.model.BestOptionRequestModel
import com.ahorrouy.ui.marketMap.model.ProductSearchModel
import com.ahorrouy.ui.marketMap.viewmodel.MarketMapViewModel
import com.ahorrouy.ui.marketMap.viewmodel.MarketMapViewModelFactory
import com.ahorrouy.ui.user.viewmodel.UserViewModel
import com.google.android.gms.location.FusedLocationProviderClient
import com.google.android.gms.location.LocationServices
import com.google.android.gms.maps.CameraUpdateFactory
import com.google.android.gms.maps.GoogleMap
import com.google.android.gms.maps.OnMapReadyCallback
import com.google.android.gms.maps.SupportMapFragment
import com.google.android.gms.maps.model.BitmapDescriptor
import com.google.android.gms.maps.model.BitmapDescriptorFactory
import com.google.android.gms.maps.model.LatLng
import com.google.android.gms.maps.model.MarkerOptions
import com.google.android.material.snackbar.Snackbar

class MarketMapFragment : Fragment(), OnMapReadyCallback {
    private var _binding: FragmentMapBinding? = null
    private val binding get() = _binding!!
    private var map: GoogleMap? = null
    private var lastKnownLocation: Location? = null
    private val DEFAULT_ZOOM = 13
    private lateinit var fusedLocationProviderClient: FusedLocationProviderClient
    private val defaultLocation = LatLng(-33.8523341, 151.2106085)
    private var longitude: Float = 0.0f
    private var latitude: Float = 0.0f
    private var productsToBuyList = ArrayList<ProductSearchModel>()
    private lateinit var marketForBuy: BestOptionRequestModel
    private lateinit var listViewMarkets: ListView
    private var adapter: MyAdapter? = null
    private lateinit var marketsOptions: List<BestOptionResponse>
    private val userVM: UserViewModel by activityViewModels()
    private val cartVM: CartViewModel by activityViewModels()


    private val marketMapVM by viewModels<MarketMapViewModel> {
        MarketMapViewModelFactory(BestOptionRepository(), PurchaseRepository())
    }

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        _binding = FragmentMapBinding.inflate(inflater, container, false)
        fusedLocationProviderClient =
            LocationServices.getFusedLocationProviderClient(requireContext())
        val mMapFragment = SupportMapFragment.newInstance()
        childFragmentManager.beginTransaction().add(R.id.map, mMapFragment).commit()
        mMapFragment.getMapAsync(this)
        productsToBuyList =
            arguments?.getParcelableArrayList("List")!!
        val toolbar = (context as Activity).findViewById<Toolbar>(R.id.toolbar)
        toolbar?.title = "Supermercados sugeridos"
        return binding.root
    }

    override fun onMapReady(googleMap: GoogleMap) {
        listViewMarkets = binding.marketsListView
        map = googleMap
        handleClickOnMarket()
        updateLocationUI()
        getDeviceLocation()
        handleDistanceOfMarkets()
        observeOptions(binding.root)
    }

    private fun handleClickOnMarket() {
        listViewMarkets.onItemClickListener =
            AdapterView.OnItemClickListener { parent, view, position, id ->
                val selectedMarket = marketsOptions[position]
                findNavController().navigate(
                    MarketMapFragmentDirections.navMapToNavHome()
                )
                cartVM.clean()
                val snack = Snackbar.make(
                    view,
                    "Lo espera el supermercado ${selectedMarket.marketName} " +
                            "para retirar su compra con un valor de ${selectedMarket.priceForProducts}",
                    Snackbar.LENGTH_LONG
                )
                snack.show()
                val purchaseModel = PurchaseModelRequest(
                    selectedMarket.priceForProducts.toInt(),
                    selectedMarket.marketName,
                    selectedMarket.marketAddress
                )
                marketMapVM.postPurchase(userVM.currentUser.value!!.Token, purchaseModel)
                handlePurchaseResponse(view, selectedMarket)
            }
    }

    private fun handlePurchaseResponse(view: View, selectedMarket: BestOptionResponse) {
        marketMapVM.purchaseResponse.observe(viewLifecycleOwner, { response ->
            if (response.isSuccessful) {
                Snackbar.make(
                    view,
                    "Lo espera el supermercado ${selectedMarket.marketName} " +
                            "para retirar su compra con un valor de ${selectedMarket.priceForProducts}",
                    Snackbar.LENGTH_LONG
                ).setAction("OK") {}.show()
            } else {
                var errorCode = response.raw().code.toString()
                if (response.headers()["ErrorCode"] != null)
                    errorCode = response.headers()["ErrorCode"] as String
                handleResponseError(errorCode, view)
            }
        })
    }

    private fun handleDistanceOfMarkets() {
        binding.sliderDistance.addOnChangeListener { slider, value, fromUser ->
            map!!.clear()
            val token = userVM.currentUser.value!!.Token
            marketForBuy =
                BestOptionRequestModel(
                    token,
                    productsToBuyList.toTypedArray(),
                    longitude,
                    latitude,
                    value.toInt()
                )
            marketMapVM.setData(marketForBuy)
        }
    }

    private fun observeOptions(view: View) {
        marketMapVM.fetchOptionsToBuy.observe(viewLifecycleOwner, Observer { response ->
            if (response.isSuccessful) {
                marketsOptions = response.body()!!
                adapter = MyAdapter(requireContext(), marketsOptions)
                listViewMarkets.adapter = adapter
                var count = 0
                marketsOptions.forEach {
                    map?.apply {
                        val marketLocation =
                            LatLng(it.marketLatitude.toDouble(), it.marketLongitude.toDouble())
                        var marker = addMarker(
                            MarkerOptions()
                                .position(marketLocation)
                                .title(it.marketName)
                                .snippet("\$ ${it.priceForProducts}")
                        )
                        if (count == 0) {
                            marker.setIcon(
                                bitmapDescriptorFromVector(
                                    requireContext(),
                                    R.drawable.ic_baseline_star_24
                                )
                            )
                        } else {
                            marker.setIcon(
                                bitmapDescriptorFromVector(
                                    requireContext(),
                                    R.drawable.ic_baseline_store_24
                                )
                            )
                        }
                        marker.showInfoWindow()
                        marker.hideInfoWindow()
                    }
                    count++
                }
            } else {
                var errorCode = response.raw().code.toString()
                if (response.headers()["ErrorCode"] != null)
                    errorCode = response.headers()["ErrorCode"] as String
                handleResponseError(errorCode, view)
            }
        })
    }

    private fun handleResponseError(errorCode: String, view: View) {
        var errorMsg: String = errorCode
        when (errorCode) {
            "500" -> errorMsg = "Error: No se pudo conectar al backend."
            "ERR_OBTEINED_BEST_OPTION" -> errorMsg =
                "No se han encontrado supermercados para los productos seleccionados."
            "ERR_FOUNDING_USER_TO_SAVE_PURCHASE" -> errorMsg =
                "Error. No se ha encontrado el usuario para asociar la compra."
            "ERR_CAN_NOT_CONNECT_DATABASE" -> errorMsg =
                "Ha ocurrido un error inesperado en el sistema."
        }
        Snackbar.make(view, errorMsg, Snackbar.LENGTH_SHORT).setAction("OK") {}.show()
    }

    private fun bitmapDescriptorFromVector(context: Context, vectorResId: Int): BitmapDescriptor {
        var vectorDrawable = ContextCompat.getDrawable(context, vectorResId)
        vectorDrawable!!.setBounds(
            0,
            0,
            vectorDrawable.intrinsicWidth,
            vectorDrawable.intrinsicHeight
        )
        var bitmap = Bitmap.createBitmap(
            vectorDrawable.intrinsicWidth,
            vectorDrawable.intrinsicHeight,
            Bitmap.Config.ARGB_8888
        )
        var canvas = Canvas(bitmap)
        vectorDrawable.draw(canvas)
        return BitmapDescriptorFactory.fromBitmap(bitmap)
    }

    private fun updateLocationUI() {
        if (map == null) {
            return
        }
        try {
            map?.isMyLocationEnabled = true
            map?.uiSettings?.isMyLocationButtonEnabled = true
        } catch (e: SecurityException) {
            Log.e("Exception: %s", e.message, e)
        }
    }

    private fun getDeviceLocation() {
        try {
            val locationResult = fusedLocationProviderClient.lastLocation
            locationResult.addOnCompleteListener(requireActivity()) { task ->
                if (task.isSuccessful) {
                    // Set the map's camera position to the current location of the device.
                    lastKnownLocation = task.result
                    if (lastKnownLocation != null) {
                        map?.moveCamera(
                            CameraUpdateFactory.newLatLngZoom(
                                LatLng(
                                    lastKnownLocation!!.latitude,
                                    lastKnownLocation!!.longitude
                                ), DEFAULT_ZOOM.toFloat()
                            )
                        )
                        longitude = lastKnownLocation!!.longitude.toFloat()
                        latitude = lastKnownLocation!!.latitude.toFloat()
                        val token = userVM.currentUser.value!!.Token
                        marketForBuy =
                            BestOptionRequestModel(
                                token,
                                productsToBuyList.toTypedArray(),
                                longitude,
                                latitude,
                                binding.sliderDistance.value.toInt()
                            )
                        marketMapVM.setData(marketForBuy)
                    }
                } else {
                    Log.d(TAG, "Current location is null. Using defaults.")
                    Log.e(TAG, "Exception: %s", task.exception)
                    map?.moveCamera(
                        CameraUpdateFactory
                            .newLatLngZoom(defaultLocation, DEFAULT_ZOOM.toFloat())
                    )
                    map?.uiSettings?.isMyLocationButtonEnabled = false
                }
            }
        } catch (e: SecurityException) {
            Log.e("Exception: %s", e.message, e)
        }
    }
}