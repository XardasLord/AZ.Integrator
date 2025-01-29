/** @type {import('tailwindcss').Config} */
var colors = require('tailwindcss/colors');

module.exports = {
  content: ['./src/**/*.{html,ts}'],
  theme: {
    extend: {
      colors: {
        transparent: 'transparent',
        primary: {
          DEFAULT: '#003068',
          light: '#3f5996',
          dark: '#00073d',
          50: '#e0e6ed',
          100: '#b3c1d2',
          200: '#8098b4',
          300: '#3f5996',
          400: '#264f7f',
          500: '#003068',
          600: '#002b60',
          700: '#002455',
          800: '#001e4b',
          900: '#00073d',
          A100: '#708fff',
          A200: '#3d67ff',
          A400: '#0a3fff',
          A700: '#0033f0',
          textDark: '#000000',
          textLight: '#ffffff',
        },
        accent: {
          DEFAULT: '#427cc3',
          light: '#78abf6',
          dark: '#005092',
          50: '#e8eff8',
          100: '#c6d8ed',
          200: '#a1bee1',
          300: '#78abf6',
          400: '#5e90cc',
          500: '#427cc3',
          600: '#3c74bd',
          700: '#3369b5',
          800: '#2b5fae',
          900: '#005092',
          A100: '#dae6ff',
          A200: '#a7c3ff',
          A400: '#74a1ff',
          A700: '#5a90ff',
          textDark: '#000000',
          textLight: '#ffffff',
        },
        gray: {
          DEFAULT: '#B4B5B6',
          bg: '#F4F5F6',
          text: '#b2b2b2',
          caption: 'rgba(0, 0, 0, 0.6)',
        },
        success: {
          DEFAULT: '#00B398',
        },
        info: {
          DEFAULT: '#A3C9DF',
        },
        warn: {
          DEFAULT: '#F0B323',
          light: '#ffc53f',
          dark: '#ce9a21',
        },
        error: {
          DEFAULT: '#FA4D56',
        },
        orange: {
          DEFAULT: '#DB864E',
        },
      },
      fontFamily: {
        sans: ['Roboto', 'Helvetica Neue', 'sans-serif'],
      },
      spacing: {
        toolbar: '4.5rem',
        sidebar: '4rem',
      },
    },
  },
  plugins: [],
};
