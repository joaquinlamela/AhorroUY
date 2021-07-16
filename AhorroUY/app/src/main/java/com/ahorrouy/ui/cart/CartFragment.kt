package com.ahorrouy.ui.cart

import android.content.Context
import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Button
import android.widget.TextView
import androidx.fragment.app.Fragment
import androidx.fragment.app.activityViewModels
import androidx.lifecycle.Observer
import androidx.recyclerview.widget.RecyclerView
import com.ahorrouy.R
import com.ahorrouy.api.model.product.ProductResponse
import com.ahorrouy.repository.model.CartProduct
import com.ahorrouy.ui.cart.adapter.CartAdapter
import com.ahorrouy.ui.cart.viewmodel.CartViewModel
import com.ahorrouy.ui.favorites.viewmodel.FavoritesViewModel
import com.ahorrouy.ui.helpers.FragmentHelper
import com.ahorrouy.ui.user.viewmodel.UserViewModel
import com.google.android.material.dialog.MaterialAlertDialogBuilder

class CartFragment : Fragment() {

    private val userVM: UserViewModel by activityViewModels()
    private val favoritesVM: FavoritesViewModel by activityViewModels()
    private val cartVM: CartViewModel by activityViewModels()

    private val cartAdapter =
        CartAdapter(::productRemoveClick, ::productNewQuantity, ::productFavOnClick)

    private lateinit var btnEmptyCart: Button

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        val root = inflater.inflate(R.layout.fragment_cart, container, false)
        btnEmptyCart = root.findViewById(R.id.btn_emptyCart)
        initRecyclerView(root)
        observeData(root)
        setupButtons()
        return root
    }

    private fun setupButtons() {
        btnEmptyCart.visibility = View.INVISIBLE
        btnEmptyCart.setOnClickListener {
            MaterialAlertDialogBuilder(activity as Context)
                .setTitle("Desea vaciar el carrito?")
                .setMessage("Se eliminarán ${cartVM.cart.value?.size} elementos del carrito")
                .setNegativeButton("Cancelar") { _, _ ->
                }
                .setPositiveButton("Aceptar") { _, _ ->
                    cartVM.clean()
                    cartAdapter.notifyDataSetChanged()
                    btnEmptyCart.visibility = View.INVISIBLE
                }
                .show()
        }
    }

    private fun observeData(root: View) {
        val emptyCartTextView: TextView = root.findViewById(R.id.txt_emptyCart)
        cartVM.cart.observe(viewLifecycleOwner, Observer { cart ->
            btnEmptyCart.visibility = View.INVISIBLE
            if (cart.size > 0) {
                btnEmptyCart.visibility = View.VISIBLE
                val favorites = getFavorites()
                cart.forEach { p ->
                    val item = favorites?.find { it.id == p.id }
                    p.isFavorite = item != null
                }
            }
            cartAdapter.submitList(cart)
            if (cart.count() == 0) emptyCartTextView.visibility = View.VISIBLE
            else emptyCartTextView.visibility = View.INVISIBLE
        })
    }

    private fun initRecyclerView(root: View) {
        val recyclerView: RecyclerView = root.findViewById(R.id.recycler_view_cart)
        recyclerView.adapter = cartAdapter
    }

    private fun getFavorites(): MutableList<ProductResponse>? {
        return FragmentHelper.favoritesGet(userVM, favoritesVM)
    }

    private fun productRemoveClick(product: CartProduct) {
        MaterialAlertDialogBuilder(activity as Context)
            .setTitle("Desea eliminar el producto?")
            .setMessage("Se eliminará ${product.name} del carrito")
            .setNegativeButton("Cancelar") { _, _ ->
            }
            .setPositiveButton("Aceptar") { _, _ ->
                cartVM.remove(product)
                cartAdapter.notifyDataSetChanged()
            }
            .show()
    }

    private fun productNewQuantity(product: CartProduct) {
        cartVM.changeQuantity(product)
    }

    private fun productFavOnClick(product: CartProduct, fav: Boolean) {
        val token = userVM.currentUser.value!!.Token
        if (fav) favoritesVM.add(token, product.id)
        else favoritesVM.remove(token, product.id)
    }
}