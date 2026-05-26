import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import ZwierzetaView from '@/views/ZwierzetaView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: HomeView,
    },
    {
      path: '/zwierzeta',
      name: 'zwierzeta',
      component: ZwierzetaView,
    },
  ],
})

export default router
