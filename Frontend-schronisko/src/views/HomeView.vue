<template>
  <div class="container mx-auto p-6 max-w-6xl animate-fade-in">
    <div
      class="hero bg-gradient-to-r from-green-50 to-emerald-100 rounded-3xl p-8 md:p-12 mb-12 shadow-sm flex flex-col lg:flex-row gap-8 items-center"
    >
      <div class="flex-1 text-center lg:text-left">
        <span class="badge badge-success text-white font-semibold px-4 py-3 mb-4"
          >{{$t('home.hero.badge') }}</span
        >
        <h1 class="text-4xl md:text-5xl font-black text-gray-800 leading-tight mb-4">
          {{$t('home.hero.title') }} <br />
          <span class="text-emerald-600">"Nasze Ogonki"</span>
        </h1>
        <p class="text-lg text-gray-600 mb-6 max-w-lg">
          {{$t('home.hero.description') }}
        </p>
        <div class="flex flex-wrap gap-4 justify-center lg:justify-start">
          <RouterLink
            to="/zwierzeta"
            class="bg-[#22c55e] hover:bg-[#16a34a] text-white font-bold py-3 px-8 rounded-xl shadow-md transition-all transform hover:-translate-y-0.5"
          >
            {{$t('home.hero.btn_meet') }}
          </RouterLink>
          <a href="#kontakt" class="btn btn-outline btn-md rounded-xl font-bold px-6">
            {{$t('home.hero.btn_contact') }}
          </a>
        </div>
      </div>

      <div class="flex-1 w-full max-w-md relative group">
        <div class="relative h-80 overflow-hidden rounded-2xl shadow-xl bg-gray-200">
          <img
            :src="zdjecia[aktywneZdjecie]"
            alt="Podopieczni schroniska"
            class="w-full h-full object-cover transition-all duration-500 transform scale-100 group-hover:scale-105"
          />
          <div class="absolute inset-0 bg-gradient-to-t from-black/40 to-transparent"></div>
        </div>

        <button
          @click="poprzedniSlajd"
          class="absolute left-3 top-1/2 -translate-y-1/2 bg-white/80 hover:bg-white text-gray-800 btn btn-circle btn-sm shadow-md border-none"
        >
          ❮
        </button>
        <button
          @click="nastepnySlajd"
          class="absolute right-3 top-1/2 -translate-y-1/2 bg-white/80 hover:bg-white text-gray-800 btn btn-circle btn-sm shadow-md border-none"
        >
          ❯
        </button>

        <div class="flex justify-center gap-1.5 mt-4">
          <span
            v-for="(zdjecie, index) in zdjecia"
            :key="index"
            class="h-2 rounded-full transition-all duration-300"
            :class="aktywneZdjecie === index ? 'w-6 bg-emerald-600' : 'w-2 bg-gray-300'"
          ></span>
        </div>
      </div>
    </div>

    <div class="grid grid-cols-1 md:grid-cols-3 gap-6 mb-16 text-center">
      <div class="bg-base-100 p-6 rounded-2xl border border-gray-100 shadow-sm">
        <div class="text-4xl mb-2">🐾</div>
        <div class="text-2xl font-black text-gray-800">100+</div>
        <div class="text-sm text-gray-500 font-medium mt-1">{{$t('home.stats.animals') }}</div>
      </div>
      <div class="bg-base-100 p-6 rounded-2xl border border-gray-100 shadow-sm">
        <div class="text-4xl mb-2">🏠</div>
        <div class="text-2xl font-black text-gray-800">450+</div>
        <div class="text-sm text-gray-500 font-medium mt-1">{{$t('home.stats.adoptions') }}</div>
      </div>
      <div class="bg-base-100 p-6 rounded-2xl border border-gray-100 shadow-sm">
        <div class="text-4xl mb-2">❤️</div>
        <div class="text-2xl font-black text-gray-800">15+</div>
        <div class="text-sm text-gray-500 font-medium mt-1">{{$t('home.stats.volunteers') }}</div>
      </div>
    </div>

    <div
      class="bg-gradient-to-br from-gray-900 to-slate-800 text-white rounded-3xl p-8 md:p-10 mb-16 shadow-lg relative overflow-hidden"
    >
      <div class="max-w-2xl mx-auto text-center">
        <h2 class="text-2xl md:text-3xl font-black mb-2">{{$t('home.quiz.title') }}</h2>
        <p class="text-gray-400 text-sm md:text-base mb-8">
          {{$t('home.quiz.description') }}
        </p>

        <div
          class="bg-slate-800/80 backdrop-blur border border-slate-700 rounded-2xl p-6 min-h-[220px] flex flex-col justify-center items-center"
        >
          <div v-if="aktualnyKrokQuizu === 0" class="w-full">
            <h3 class="text-lg font-bold mb-4 text-emerald-400">{{$t('home.quiz.step_0.question') }}</h3>
            <div class="flex flex-col sm:flex-row gap-3 justify-center">
              <button
                @click="nastepnyKrok('miejsce', 'dom')"
                class="btn btn-outline btn-accent text-white border-slate-600 hover:bg-emerald-600 hover:border-emerald-600 px-8 rounded-xl flex-1"
              >
                {{$t('home.quiz.step_0.house') }}
              </button>
              <button
                @click="nastepnyKrok('miejsce', 'mieszkanie')"
                class="btn btn-outline btn-accent text-white border-slate-600 hover:bg-emerald-600 hover:border-emerald-600 px-8 rounded-xl flex-1"
              >
                {{$t('home.quiz.step_0.apartment') }}
              </button>
            </div>
          </div>

          <div v-if="aktualnyKrokQuizu === 1" class="w-full">
            <h3 class="text-lg font-bold mb-4 text-emerald-400">
              {{$t('home.quiz.step_1.question') }}
            </h3>
            <div class="flex flex-col sm:flex-row gap-3 justify-center">
              <button
                @click="nastepnyKrok('czas', 'duzo')"
                class="btn btn-outline btn-accent text-white border-slate-600 hover:bg-emerald-600 hover:border-emerald-600 px-8 rounded-xl flex-1"
              >
                {{$t('home.quiz.step_1.lot_of_time') }}
              </button>
              <button
                @click="nastepnyKrok('czas', 'malo')"
                class="btn btn-outline btn-accent text-white border-slate-600 hover:bg-emerald-600 hover:border-emerald-600 px-8 rounded-xl flex-1"
              >
                {{$t('home.quiz.step_1.little_time') }}
              </button>
            </div>
          </div>

          <div v-if="aktualnyKrokQuizu === 2" class="w-full text-center">
            <div class="text-4xl mb-3 animate-bounce">✨</div>
            <h3 class="text-xl font-black text-emerald-400 mb-2">{{$t('home.quiz.step_2.result_title') }}</h3>
            <p class="text-lg font-medium text-white max-w-md mx-auto mb-6">{{ wynikQuizu }}</p>
            <button
              @click="restartujQuiz"
              class="btn btn-sm btn-ghost text-emerald-400 hover:bg-emerald-500/10 rounded-lg"
            >
              {{$t('home.quiz.step_2.restart') }}
            </button>
          </div>
        </div>
      </div>
    </div>

    <div id="kontakt" class="bg-base-200 rounded-3xl p-8 md:p-10 shadow-sm border border-gray-100">
      <div class="text-center max-w-xl mx-auto mb-10">
        <h2 class="text-3xl font-black text-gray-800 mb-2">{{$t('home.contact.title') }}</h2>
        <p class="text-gray-500">
          {{$t('home.contact.description') }}
        </p>
      </div>

      <div class="flex flex-col lg:flex-row gap-8">
        <div class="flex-1 grid grid-cols-1 sm:grid-cols-2 gap-4">
          <div
            class="bg-base-100 p-5 rounded-2xl shadow-sm border border-gray-100 flex items-start gap-4"
          >
            <div class="p-3 bg-emerald-50 text-emerald-600 rounded-xl text-xl">📞</div>
            <div class="min-w-0 flex-1">
              <h4 class="font-bold text-gray-800 text-sm">{{$t('home.contact.phone.title') }}</h4>
              <p class="text-emerald-600 font-semibold mt-1 break-all">+48 500 600 700</p>
              <p class="text-xs text-gray-400 mt-0.5">{{$t('home.contact.phone.hours') }}</p>
            </div>
          </div>

          <div
            class="bg-base-100 p-5 rounded-2xl shadow-sm border border-gray-100 flex items-start gap-4"
          >
            <div class="p-3 bg-emerald-50 text-emerald-600 rounded-xl text-xl">📍</div>
            <div class="min-w-0 flex-1">
              <h4 class="font-bold text-gray-800 text-sm">{{$t('home.contact.address.title') }}</h4>
              <p class="text-gray-700 font-medium mt-1 break-all">ul. Zwierzyniecka 12</p>
              <p class="text-xs text-gray-500">80-000 Gdańsk</p>
            </div>
          </div>

          <div
            class="bg-base-100 p-5 rounded-2xl shadow-sm border border-gray-100 flex items-start gap-4"
          >
            <div class="p-3 bg-emerald-50 text-emerald-600 rounded-xl text-xl">✉️</div>
            <div class="min-w-0 flex-1">
              <h4 class="font-bold text-gray-800 text-sm">{{$t('home.contact.email.title') }}</h4>
              <p class="text-emerald-600 font-semibold mt-1 break-all">kontakt@naszeogonki.pl</p>
              <p class="text-xs text-gray-400 mt-0.5">{{$t('home.contact.email.response_time') }}</p>
            </div>
          </div>

          <div
            class="bg-base-100 p-5 rounded-2xl shadow-sm border border-gray-100 flex items-start gap-4"
          >
            <div class="p-3 bg-emerald-50 text-emerald-600 rounded-xl text-xl">💚</div>
            <div class="min-w-0 flex-1">
              <h4 class="font-bold text-gray-800 text-sm">{{$t('home.contact.krs.title') }}</h4>
              <p class="text-gray-700 font-mono font-bold mt-1 break-all">0000123456</p>
              <p class="text-xs text-gray-500">{{$t('home.contact.krs.purpose') }}</p>
            </div>
          </div>
        </div>

        <div
          class="flex-1 rounded-2xl overflow-hidden shadow-sm min-h-[240px] border border-gray-200 relative"
        >
          <iframe
            src="https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d2325.2755255013054!2d18.63821037726487!3d54.35242509935102!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x46fd73a11634b3f5%3A0xe2107b30fb0d2f09!2zR2RhxYRsayBHxYLDs3dueQ!5e0!3m2!1spl!2spl!4v1710000000000!5m2!1spl!2spl"
            class="w-full h-full min-h-[250px] border-0"
            allowfullscreen="true"
            loading="lazy"
            referrerpolicy="no-referrer-when-downgrade"
          ></iframe>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { useZwierzetaStore } from '@/stores/zwierzeta'
import { useI18n } from 'vue-i18n'

const { t } = useI18n()

const zwierzetaStore = useZwierzetaStore()
const aktywneZdjecie = ref(0)

const aktualnyKrokQuizu = ref(0)
const wynikQuizu = ref('')
const preferencje = ref({
  miejsce: '',
  czas: '',
})

const zdjecia = computed(() => {
  const listaZdjecZazywca = zwierzetaStore.zwierzeta
    .map(
      (z) => (z as any).zdjecieUrl || (z as any).fotoUrl || (z as any).zdziecie || (z as any).foto,
    )
    .filter(Boolean)

  if (listaZdjecZazywca.length > 0) {
    return listaZdjecZazywca
  }

  return [
    'https://images.unsplash.com/photo-1543466835-00a7907e9de1?auto=format&fit=crop&w=800&q=80',
    'https://images.unsplash.com/photo-1514888286974-6c03e2ca1dba?auto=format&fit=crop&w=800&q=80',
    'https://images.unsplash.com/photo-1537151625747-768eb64519f2?auto=format&fit=crop&w=800&q=80',
    'https://images.unsplash.com/photo-1573865526739-10659fec78a5?auto=format&fit=crop&w=800&q=80',
  ]
})

onMounted(async () => {
  if (zwierzetaStore.zwierzeta.length === 0) {
    await zwierzetaStore.pobierzZwierzeta()
  }
})

const restartujQuiz = () => {
  aktualnyKrokQuizu.value = 0
  wynikQuizu.value = ''
  preferencje.value = { miejsce: '', czas: '' }
}

const nastepnyKrok = (klucz: 'miejsce' | 'czas', wartosc: string) => {
  preferencje.value[klucz] = wartosc
  if (aktualnyKrokQuizu.value < 1) {
    aktualnyKrokQuizu.value++
  } else {
    if (preferencje.value.miejsce === 'dom' && preferencje.value.czas === 'duzo') {
      wynikQuizu.value = t('home.quiz.step_2.results.active_dog') 
    } else if (preferencje.value.miejsce === 'mieszkanie' && preferencje.value.czas === 'malo') {
      wynikQuizu.value = t('home.quiz.step_2.results.calm_cat') 
    } else if (preferencje.value.czas === 'malo') {
      wynikQuizu.value = t('home.quiz.step_2.results.independent') 
    } else {
      wynikQuizu.value = t('home.quiz.step_2.results.young') 
    }
    aktualnyKrokQuizu.value = 2
  }
}

const nastepnySlajd = () => {
  aktywneZdjecie.value = (aktywneZdjecie.value + 1) % zdjecia.value.length
}

const poprzedniSlajd = () => {
  aktywneZdjecie.value = (aktywneZdjecie.value - 1 + zdjecia.value.length) % zdjecia.value.length
}
</script>
