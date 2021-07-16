package com.ahorrouy.ui.favorites

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.TextView
import androidx.fragment.app.Fragment
import androidx.fragment.app.activityViewModels
import androidx.lifecycle.Observer
import androidx.recyclerview.widget.GridLayoutManager
import androidx.recyclerview.widget.RecyclerView
import com.ahorrouy.R
import com.ahorrouy.api.model.product.ProductResponse
import com.ahorrouy.repository.model.CartProduct
import com.ahorrouy.ui.cart.viewmodel.CartViewModel
import com.ahorrouy.ui.favorites.adapter.FavoritesAdapter
import com.ahorrouy.ui.favorites.viewmodel.FavoritesViewModel
import com.ahorrouy.ui.helpers.FragmentHelper
import com.ahorrouy.ui.user.viewmodel.UserViewModel

class FavoritesFragment : Fragment() {

    private val userVM: UserViewModel by activityViewModels()
    private val favoritesVM: FavoritesViewModel by activityViewModels()
    private val cartVM: CartViewModel by activityViewModels()

    private val favoritesAdapter = FavoritesAdapter(::productFavOnClick, ::productAddCartOnClick)

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        val root = inflater.inflate(R.layout.fragment_favorites, container, false)
        initRecyclerView(root)
        return root
    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        observeData(view)
    }

    private fun initRecyclerView(view: View) {
        val recyclerView: RecyclerView = view.findViewById(R.id.recycler_view_favorites)
        recyclerView.adapter = favoritesAdapter
        recyclerView.layoutManager = GridLayoutManager(context, 2)
    }

    private fun observeData(view: View) {
        val noFavoritesTextView: TextView = view.findViewById(R.id.txt_noFavorites)
        favoritesVM.favorites.observe(viewLifecycleOwner, Observer { favorites ->
            if (favorites.isEmpty()) {
                noFavoritesTextView.visibility = View.VISIBLE
            }
            favoritesAdapter.submitList(favorites)
            favoritesAdapter.notifyDataSetChanged()
            /*
                val errorCode = response.headers()["ErrorCode"] as String
                Toast.makeText(
                    requireContext(),
                    "Error obteniendo productos favoritos $errorCode",
                    Toast.LENGTH_LONG
                ).show()*/
        })
    }

    private fun productFavOnClick(product: ProductResponse) {
        val token = userVM.currentUser.value!!.Token
        favoritesVM.remove(token, product.id)
    }

    private fun productAddCartOnClick(view: View, product: ProductResponse, quantity: Int) {
        val cartProduct = CartProduct.createFromProductResponse(product, quantity)
        FragmentHelper.productAddCartOnClick(view, userVM, cartVM, cartProduct)
    }
}