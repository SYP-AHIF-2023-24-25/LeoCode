const MonacoWebpackPlugin = require('monaco-editor-webpack-plugin');

module.exports = {
  entry: './src/index.ts',
  output: {
    filename: 'bundle.js',
    path: __dirname + '/dist',
  },
  resolve: {
    extensions: ['.ts', '.tsx', '.js'],
  },
  module: {
    rules: [
      {
        test: /\.tsx?$/,
        use: 'ts-loader',
        exclude: /node_modules/,
      },
      // Add any other loaders or rules you need
    ],
  },
  plugins: [
    new MonacoWebpackPlugin({
      // Languages you want to support
      languages: ['typescript'],
    }),
  ],
  // Add any other configuration options you need
};
