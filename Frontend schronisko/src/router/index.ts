import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import ZwierzetaView from '@/views/ZwierzetaView.vue'
import ZwierzeSzczegolyView from '../views/ZwierzeSzczegolyView.vue'

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
    {
      path: '/zwierze/:id',
      name: 'zwierze-szczegoly',
      component: ZwierzeSzczegolyView,
    },
  ],
})

export default router
