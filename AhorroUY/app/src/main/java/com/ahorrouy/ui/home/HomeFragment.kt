package com.ahorrouy.ui.home

import android.content.Context
import android.content.Intent
import android.content.SharedPreferences
import android.os.Bundle
import android.util.Log
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.SearchView
import android.widget.Toast
import androidx.fragment.app.Fragment
import androidx.fragment.app.activityViewModels
import androidx.lifecycle.Observer
import androidx.navigation.fragment.findNavController
import androidx.recyclerview.widget.LinearLayoutManager
import com.ahorrouy.api.Constants.Companion.DEFAULT_BARCODE
import com.ahorrouy.api.Constants.Companion.DEFAULT_CATEGORY
import com.ahorrouy.api.Constants.Companion.DEFAULT_PRODUCT_NAME
import com.ahorrouy.api.model.category.CategoryResponse
import com.ahorrouy.api.model.product.ProductDiscountResponse
import com.ahorrouy.api.model.product.ProductResponse
import com.ahorrouy.databinding.FragmentHomeBinding
import com.ahorrouy.repository.model.CartProduct
import com.ahorrouy.ui.cart.viewmodel.CartViewModel
import com.ahorrouy.ui.favorites.viewmodel.FavoritesViewModel
import com.ahorrouy.ui.helpers.FragmentHelper
import com.ahorrouy.ui.home.adapter.CategoriesAdapter
import com.ahorrouy.ui.home.adapter.ProductDiscountAdapter
import com.ahorrouy.ui.home.viewmodel.HomeViewModel
import com.ahorrouy.ui.search.adapter.ProductSearchAdapter
import com.ahorrouy.ui.user.viewmodel.UserViewModel
import com.google.android.material.snackbar.Snackbar
import com.google.zxing.integration.android.IntentIntegrator

class HomeFragment : Fragment() {

    private var _binding: FragmentHomeBinding? = null
    private val binding get() = _binding!!
    private val userVM: UserViewModel by activityViewModels()
    private val favoritesVM: FavoritesViewModel by activityViewModels()
    private val cartVM: CartViewModel by activityViewModels()
    private val homeVM: HomeViewModel by activityViewModels()

    private lateinit var prodDiscAdapter: ProductDiscountAdapter
    private lateinit var productsSearchAdapter: ProductSearchAdapter

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        _binding = FragmentHomeBinding.inflate(inflater, container, false)
        return binding.root
    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)
        binding.searchBar.setQuery(DEFAULT_PRODUCT_NAME, false)
        initRecyclerViewForCategories()
        initRecyclerViewForProducts()
        initRecyclerViewForDiscountProducts()
        setUpSearchBar()
        setUpObservers(view)
        handleScanClickButton()
    }

    override fun onActivityResult(requestCode: Int, resultCode: Int, data: Intent?) {
        val result = IntentIntegrator.parseActivityResult(requestCode, resultCode, data)
        if (result != null) {
            if (result.contents == null) {
                Toast.makeText(
                    context,
                    "Ha cancelado la busqueda por codigo de barras.",
                    Toast.LENGTH_LONG
                ).show()
            } else {
                findNavController().navigate(
                    HomeFragmentDirections.homeToSearchProduct(
                        DEFAULT_PRODUCT_NAME,
                        result.contents,
                        DEFAULT_CATEGORY
                    )
                )
            }
        } else {
            super.onActivityResult(requestCode, resultCode, data)
        }
    }

    override fun onPause() {
        super.onPause()
        sendToken()
    }

    private fun setUpObservers(view: View) {
        val isLoggedIn = userVM.currentUser.value != null
        prodDiscAdapter = ProductDiscountAdapter(
            isLoggedIn,
            ::productDiscountAddCartClick,
            ::productDiscountFavOnClick
        )
        binding.rvDiscountedItems.adapter = prodDiscAdapter
        observeDiscountProducts(prodDiscAdapter, view)
        val catAdapter = CategoriesAdapter(::onCategoryClick)
        binding.rvCategories.adapter = catAdapter
        observeCategories(catAdapter, view)
        productsSearchAdapter =
            ProductSearchAdapter(isLoggedIn, ::productAddCartClick, ::productFavOnClick)
        binding.rvProducts.adapter = productsSearchAdapter
        observeProducts(productsSearchAdapter, view)
        observeFavorites()
    }

    private fun observeFavorites() {
        favoritesVM.favorites.observe(viewLifecycleOwner, Observer { favorites ->
            prodDiscAdapter.currentList.forEach {
                it.isFavorite = favorites.any { f -> f.id == it.id }
            }
            prodDiscAdapter.notifyDataSetChanged()
            productsSearchAdapter.currentList.forEach {
                it.isFavorite = favorites.any { f -> f.id == it.id }
            }
            productsSearchAdapter.notifyDataSetChanged()
        })
    }

    private fun observeDiscountProducts(prodDiscAdapter: ProductDiscountAdapter, view: View) {
        homeVM.fetchDiscountedProductList.observe(viewLifecycleOwner, Observer { response ->
            if (response.isSuccessful) {
                val favorites = getFavorites()
                val list = response.body()
                list?.forEach {
                    it.isFavorite = favorites!!.any { f -> f.id == it.id }
                }
                prodDiscAdapter.submitList(list)
            } else {
                var errorCode = response.raw().code.toString()
                if (response.headers()["ErrorCode"] != null)
                    errorCode = response.headers()["ErrorCode"] as String
                handleResponseError(errorCode, view)
            }
        })
    }

    private fun observeProducts(productsSearchAdapter: ProductSearchAdapter, view: View) {
        homeVM.fetchProductList.observe(viewLifecycleOwner, Observer { response ->
            if (response.isSuccessful) {
                val favorites = getFavorites()
                val list = response.body()
                list?.forEach {
                    it.isFavorite = favorites!!.any { f -> f.id == it.id }
                }
                productsSearchAdapter.submitList(list)
            } else {
                var errorCode = response.raw().code.toString()
                if (response.headers()["ErrorCode"] != null)
                    errorCode = response.headers()["ErrorCode"] as String
                handleResponseError(errorCode, view)
            }
        })
    }

    private fun observeCategories(catAdapter: CategoriesAdapter, view: View) {
        homeVM.fetchCategoriesList.observe(viewLifecycleOwner, Observer { response ->
            if (response.isSuccessful) {
                catAdapter.submitList(response.body() as List<CategoryResponse>)
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
            "ERR_CATEGORIES_NOT_FOUND" -> errorMsg =
                "Error: No se han encontrado categorias."
            "ERR_PRODUCTS_NOT_FOUND" -> errorMsg = "No hay productos disponibles en este momento."
            "ERR_PRODUCTS_WITH_DISCOUNTS_NOT_FOUND" -> errorMsg =
                "No hay productos en descuento disponibles en este momento."
            "ERR_NOT_FOUND_MIN_AND_MAX_PRICE" -> errorMsg =
                "No se ha podido encontrar un precio minimo y maximo para el producto."
            "ERR_CAN_NOT_CONNECT_DATABASE" -> errorMsg =
                "Ha ocurrido un error inesperado en el sistema."
        }
        Snackbar.make(view, errorMsg, Snackbar.LENGTH_SHORT).setAction("OK") {}.show()
    }

    private fun initRecyclerViewForDiscountProducts() {
        binding.rvDiscountedItems.layoutManager = LinearLayoutManager(
            requireContext(),
            LinearLayoutManager.HORIZONTAL,
            false
        )
    }

    private fun initRecyclerViewForCategories() {
        binding.rvCategories.layoutManager = LinearLayoutManager(
            requireContext(),
            LinearLayoutManager.HORIZONTAL,
            false
        )
    }

    private fun initRecyclerViewForProducts() {
        binding.rvProducts.layoutManager = LinearLayoutManager(
            requireContext(),
            LinearLayoutManager.HORIZONTAL,
            false
        )
    }

    private fun sendToken() {
        val sharedPreferenceInstance =
            requireContext().getSharedPreferences("Preferences", Context.MODE_PRIVATE)
        val tokenSent = sharedPreferenceInstance.getString("tokenSent", null)
        val tokenToSend = sharedPreferenceInstance.getString("registration_id", null)

        if (tokenSent != tokenToSend) {
            homeVM.postToken(tokenToSend!!)
            val editor: SharedPreferences.Editor = sharedPreferenceInstance.edit()
            editor.putString("tokenSent", tokenToSend)
            editor.apply()
        }
    }

    private fun setUpSearchBar() {
        binding.searchBar.setOnQueryTextListener(object : SearchView.OnQueryTextListener {
            override fun onQueryTextSubmit(query: String?): Boolean {
                try {
                    findNavController().navigate(
                        HomeFragmentDirections.homeToSearchProduct(
                            query!!,
                            DEFAULT_BARCODE,
                            DEFAULT_CATEGORY
                        )
                    )
                } catch (e: IllegalArgumentException) {
                    Log.e("NAV_ERROR", "Error al navegar al fragmento de búsqueda")
                }
                return false
            }

            override fun onQueryTextChange(newText: String?): Boolean {
                return false
            }
        })
    }

    private fun handleScanClickButton() {
        val integrator = IntentIntegrator.forSupportFragment(this)
        binding.btnScan.setOnClickListener {
            integrator.setDesiredBarcodeFormats(IntentIntegrator.ALL_CODE_TYPES)
            integrator.setPrompt("Escanee el codigo de barras de su producto aqui")
            integrator.setCameraId(0)
            integrator.setBeepEnabled(true)
            integrator.setOrientationLocked(false)
            integrator.setBarcodeImageEnabled(false)
            integrator.initiateScan()
        }
    }

    private fun onCategoryClick(category: CategoryResponse) {
        findNavController().navigate(
            HomeFragmentDirections.homeToSearchProduct(
                DEFAULT_PRODUCT_NAME,
                DEFAULT_BARCODE,
                category
            )
        )
    }

    private fun getFavorites(): MutableList<ProductResponse>? {
        return FragmentHelper.favoritesGet(userVM, favoritesVM)
    }

    private fun productDiscountAddCartClick(
        view: View,
        product: ProductDiscountResponse,
        quantity: Int
    ) {
        val cartProduct = CartProduct.createFromProductDiscountResponse(product, quantity)
        FragmentHelper.productAddCartOnClick(view, userVM, cartVM, cartProduct)
    }

    private fun productDiscountFavOnClick(product: ProductDiscountResponse, fav: Boolean) {
        val token = userVM.currentUser.value!!.Token
        if (fav) favoritesVM.add(token, product.id)
        else favoritesVM.remove(token, product.id)

        val list2 = productsSearchAdapter.currentList
        val p = list2.find { it.id == product.id }
        p?.isFavorite = fav
        productsSearchAdapter.notifyDataSetChanged()
    }

    private fun productAddCartClick(view: View, product: ProductResponse, quantity: Int) {
        val cartProduct = CartProduct.createFromProductResponse(product, quantity)
        FragmentHelper.productAddCartOnClick(view, userVM, cartVM, cartProduct)
    }

    private fun productFavOnClick(product: ProductResponse, fav: Boolean) {
        val token = userVM.currentUser.value!!.Token
        if (fav) favoritesVM.add(token, product.id)
        else favoritesVM.remove(token, product.id)

        val list1 = prodDiscAdapter.currentList
        val p = list1.find { it.id == product.id }
        p?.isFavorite = fav
        prodDiscAdapter.notifyDataSetChanged()
    }
}
