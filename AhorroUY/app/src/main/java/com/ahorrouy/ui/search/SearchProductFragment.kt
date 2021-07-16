package com.ahorrouy.ui.search

import android.app.Activity
import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.AdapterView
import android.widget.ArrayAdapter
import android.widget.SearchView
import android.widget.Spinner
import androidx.appcompat.widget.Toolbar
import androidx.fragment.app.Fragment
import androidx.fragment.app.activityViewModels
import androidx.fragment.app.viewModels
import androidx.lifecycle.Observer
import androidx.recyclerview.widget.GridLayoutManager
import androidx.recyclerview.widget.LinearLayoutManager
import com.ahorrouy.R
import com.ahorrouy.api.Constants.Companion.DEFAULT_CATEGORY
import com.ahorrouy.api.model.category.CategoryResponse
import com.ahorrouy.api.model.product.ProductResponse
import com.ahorrouy.databinding.FragmentSearchProductsBinding
import com.ahorrouy.repository.implementation.CategoryRepository
import com.ahorrouy.repository.implementation.ProductRepository
import com.ahorrouy.repository.model.CartProduct
import com.ahorrouy.ui.cart.viewmodel.CartViewModel
import com.ahorrouy.ui.favorites.viewmodel.FavoritesViewModel
import com.ahorrouy.ui.helpers.FragmentHelper
import com.ahorrouy.ui.home.adapter.CategoriesAdapter
import com.ahorrouy.ui.search.adapter.ProductSearchAdapter
import com.ahorrouy.ui.search.model.Search
import com.ahorrouy.ui.search.viewmodel.SearchProductViewModel
import com.ahorrouy.ui.search.viewmodel.SearchProductViewModelFactory
import com.ahorrouy.ui.user.viewmodel.UserViewModel
import com.google.android.material.snackbar.Snackbar

class SearchProductFragment : Fragment() {
    private var _binding: FragmentSearchProductsBinding? = null
    private val binding get() = _binding!!

    private lateinit var barcode: String
    private var parametersForSearch: Search = Search()

    private val userVM: UserViewModel by activityViewModels()
    private val favoritesVM: FavoritesViewModel by activityViewModels()
    private val cartVM: CartViewModel by activityViewModels()

    private val searchVM by viewModels<SearchProductViewModel> {
        SearchProductViewModelFactory(ProductRepository(), CategoryRepository())
    }

    private lateinit var productsSearchAdapter: ProductSearchAdapter

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        _binding = FragmentSearchProductsBinding.inflate(inflater, container, false)
        return binding.root
    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)
        val toolbar = (context as Activity).findViewById<Toolbar>(R.id.toolbar)
        toolbar.title = "Búsqueda de productos"

        parametersForSearch.productName =
            SearchProductFragmentArgs.fromBundle(requireArguments()).searchText
        println("nombre de producto buscado" + parametersForSearch.productName)
        binding.searchBar.setQuery(parametersForSearch.productName, false)
        searchVM.setParameters(parametersForSearch)

        barcode = SearchProductFragmentArgs.fromBundle(requireArguments()).barcode
        var category = SearchProductFragmentArgs.fromBundle(requireArguments()).category
        parametersForSearch.categoryId = category.id
        if (parametersForSearch.productName.isNotEmpty()) {
            binding.searchBar.setQuery(parametersForSearch.productName, false)
            searchVM.setParameters(parametersForSearch)
        }
        if (barcode.isNotEmpty()) {
            searchVM.setBarcode(barcode)
        }
        if (parametersForSearch.categoryId != DEFAULT_CATEGORY.id) {
            binding.chipCategory.visibility = View.VISIBLE
            binding.txtCategory1.visibility = View.VISIBLE
            binding.chipCategory.text = " ${category.categoryName}"
            searchVM.setParameters(parametersForSearch)
        }

        initRecyclerViewForCategories()
        initRecyclerViewForProducts()
        setUpSearchedProducts(view)
        setUpProductSearchByBarcode(view)
        setUpSearchBar()
        setUpCategories(view)
        setUpCriteria()
        setUpCategoryOnClickListener()

    }

    private fun initRecyclerViewForCategories() {
        binding.rvCategories.layoutManager =
            LinearLayoutManager(requireContext(), LinearLayoutManager.HORIZONTAL, false)
    }

    private fun initRecyclerViewForProducts() {
        binding.rvSearchProducts.layoutManager = GridLayoutManager(requireContext(), 2)
    }

    private fun setUpSearchedProducts(view: View) {
        val isLoggedIn = userVM.currentUser.value != null
        productsSearchAdapter =
            ProductSearchAdapter(isLoggedIn, ::productAddCartOnClick, ::productFavOnClick)
        binding.rvSearchProducts.adapter = productsSearchAdapter
        searchVM.fetchSearchedProductList.observe(viewLifecycleOwner, Observer { response ->
            if (response.isSuccessful) {
                val favorites = getFavorites()
                val list = response.body() as MutableList<ProductResponse>
                list.forEach {
                    it.isFavorite = favorites!!.any { f -> f.id == it.id }
                }
                productsSearchAdapter.submitList(list)
            } else {
                productsSearchAdapter.submitList(emptyList())
                var errorCode = response.raw().code.toString()
                if (response.headers()["ErrorCode"] != null)
                    errorCode = response.headers()["ErrorCode"] as String
                handleResponseError(errorCode, view)
            }
        })
    }

    private fun setUpProductSearchByBarcode(view: View) {
        searchVM.fetchProductByBarcode.observe(viewLifecycleOwner, Observer { response ->
            if (response.isSuccessful) {
                val favorites = getFavorites()
                val list = response.body() as MutableList<ProductResponse>
                list.forEach {
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

    private fun setUpSearchBar() {
        binding.searchBar.setOnQueryTextListener(object : SearchView.OnQueryTextListener {
            override fun onQueryTextSubmit(query: String?): Boolean {
                parametersForSearch.productName = query!!
                searchVM.setParameters(parametersForSearch)
                return false
            }

            override fun onQueryTextChange(newText: String?): Boolean {
                if (newText.equals("")) {
                    onQueryTextSubmit("")
                }
                return true
            }
        })
    }

    private fun setUpCategories(view: View) {
        val catAdapter = CategoriesAdapter(::onCategoryClick)
        binding.rvCategories.adapter = catAdapter
        searchVM.fetchCategoriesList.observe(viewLifecycleOwner, Observer { response ->
            if (response.isSuccessful) {
                val list = response.body() as List<CategoryResponse>
                val activeCat = list.find { c -> c.id == parametersForSearch.categoryId }
                activeCat?.active = true
                catAdapter.submitList(list)
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
            "ERR_PRODUCTS_BY_SEARCH_NOT_FOUND" -> errorMsg =
                "No hay productos disponibles para la busqueda realizada."
            "ERR_PRODUCT_SEARCHED_BY_BARCODE_NOT_FOUND" -> errorMsg =
                "No se ha encontrado un producto para el codigo de barras buscado."
            "ERR_NOT_FOUND_MIN_AND_MAX_PRICE" -> errorMsg =
                "No se ha podido encontrar un precio minimo y maximo para el producto."
            "ERR_CAN_NOT_CONNECT_DATABASE" -> errorMsg =
                "Ha ocurrido un error inesperado en el sistema."
        }
        Snackbar.make(view, errorMsg, Snackbar.LENGTH_SHORT).setAction("OK") {}.show()
    }

    private fun setUpCriteria() {
        val spinner: Spinner = binding.spinnerCriteria
        ArrayAdapter.createFromResource(
            requireContext(),
            R.array.criterias,
            android.R.layout.simple_spinner_item
        ).also { adapter ->
            adapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item)
            spinner.adapter = adapter
        }
        spinner.onItemSelectedListener = object : AdapterView.OnItemSelectedListener {
            override fun onItemSelected(
                parent: AdapterView<*>,
                view: View,
                position: Int,
                id: Long
            ) {
                parametersForSearch.criteria = position
                searchVM.setParameters(parametersForSearch)
            }

            override fun onNothingSelected(parent: AdapterView<*>) {}
        }
    }

    private fun setUpCategoryOnClickListener() {
        binding.chipCategory.setOnClickListener {
            binding.chipCategory.visibility = View.INVISIBLE
            binding.txtCategory1.visibility = View.INVISIBLE
            parametersForSearch.categoryId = DEFAULT_CATEGORY.id
            searchVM.setParameters(parametersForSearch)
        }
    }

    private fun getFavorites(): List<ProductResponse>? {
        return FragmentHelper.favoritesGet(userVM, favoritesVM)
    }


    private fun productAddCartOnClick(view: View, product: ProductResponse, quantity: Int) {
        val cartProduct = CartProduct.createFromProductResponse(product, quantity)
        FragmentHelper.productAddCartOnClick(view, userVM, cartVM, cartProduct)
    }

    private fun productFavOnClick(product: ProductResponse, fav: Boolean) {
        val token = userVM.currentUser.value!!.Token
        if (fav) favoritesVM.add(token, product.id)
        else favoritesVM.remove(token, product.id)
    }

    private fun onCategoryClick(category: CategoryResponse) {
        binding.chipCategory.visibility = View.VISIBLE
        binding.txtCategory1.visibility = View.VISIBLE
        binding.chipCategory.text = " ${category.categoryName}"
        parametersForSearch.categoryId = category.id
        searchVM.setParameters(parametersForSearch)
    }
}