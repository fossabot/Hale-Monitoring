var webpack = require('webpack');
var path = require('path');

module.exports = {
    context: __dirname,
    entry: [
        'babel-polyfill',
        './src/app.js'
    ],
    output: {
        path: __dirname + '/dist',
        filename: 'app.js'
    },
    plugins: [
        new webpack.ProvidePlugin({
            $: "jquery",
            jQuery: "jquery"
        })
    ],
    module: {
        loaders: [
            { test: /\.css$/, loader: "style!css" },
            { test: /\.scss$/, loader: "style!css!sass" },
            { test: /\.woff(2)?(\?v=[0-9]\.[0-9]\.[0-9])?$/, loader: "url-loader?limit=10000&mimetype=application/font-woff" },
            { test: /\.(ttf|eot|svg)(\?v=[0-9]\.[0-9]\.[0-9])?$/, loader: "file-loader" },
            { test: /\.js$/, exclude: /node_modules/, loader: "babel", query: { presets: ['es2015']}}
        ]
    }
};