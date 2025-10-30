import axios from 'axios'

const http = axios.create({
  baseURL: '', // Use empty string for relative URLs with Vite proxy
})

export default http
