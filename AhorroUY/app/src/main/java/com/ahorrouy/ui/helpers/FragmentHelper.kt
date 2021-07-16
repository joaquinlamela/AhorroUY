package com.ahorrouy.ui.helpers

import android.view.View
import androidx.navigation.findNavController
import com.ahorrouy.R
import com.ahorrouy.api.model.product.ProductResponse
import com.ahorrouy.repository.model.CartProduct
import com.ahorrouy.ui.cart.viewmodel.CartViewModel
import com.ahorrouy.ui.favorites.viewmodel.FavoritesViewModel
import com.ahorrouy.ui.user.viewmodel.UserViewModel
import com.google.android.material.snackbar.Snackbar

class FragmentHelper {
    companion object Factory {
        fun productAddCartOnClick(
            view: View,
            userVM: UserViewModel,
            cartVM: CartViewModel,
            product: CartProduct
        ) {
            val isLoggedIn = userVM.currentUser.value != null
            if (isLoggedIn) {
                cartVM.add(product)
            } else {
                val snack = Snackbar.make(
                    view,
                    "Debe iniciar sesión para agregar al carrito",
                    Snackbar.LENGTH_SHORT
                )
                snack.setAction("Login") { view.findNavController().navigate(R.id.nav_login) }
                snack.show()
            }
        }

        fun favoritesGet(
            userVM: UserViewModel, favoritesVM: FavoritesViewModel
        ): MutableList<ProductResponse>? {
            val favorites: MutableList<ProductResponse> = arrayListOf()
            val isLoggedIn = userVM.currentUser.value != null
            if (isLoggedIn) {
                return favoritesVM.favorites.value
            }
            return favorites
        }
    }
}